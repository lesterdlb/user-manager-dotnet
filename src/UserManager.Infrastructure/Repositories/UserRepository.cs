using MapsterMapper;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using UserManager.Application.Common.Interfaces.Repositories;
using UserManager.Domain.Entities;
using UserManager.Infrastructure.Identity;

namespace UserManager.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private const string AdminRole = "Admin";

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public UserRepository(UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<User?> GetByIdAsync(Guid userId)
    {
        var appUser = await _userManager.FindByIdAsync(userId.ToString());
        return appUser is null ? null : _mapper.Map<User>(appUser);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var users = await _userManager.Users.ToListAsync();

        return _mapper.Map<List<User>>(users);
    }

    public async Task<User> AddUserWithPasswordAsync(User user, string password)
    {
        return await CreateUser(user, password);
    }

    public async Task<User> AddAsync(User user)
    {
        return await CreateUser(user);
    }

    public async Task UpdateAsync(User user)
    {
        var appUser = await _userManager.FindByIdAsync(user.Id.ToString());

        if (appUser is null) throw new ApplicationException($"Unable to find user with id {user.Id}");

        appUser.FirstName = user.FirstName;
        appUser.LastName = user.LastName;
        appUser.Email = user.Email;

        var result = await _userManager.UpdateAsync(appUser);

        if (!result.Succeeded)
            throw new ApplicationException($"Unable to update user: {result.Errors.FirstOrDefault()?.Description}");
    }

    public async Task DeleteAsync(Guid userId)
    {
        var appUser = await _userManager.FindByIdAsync(userId.ToString());

        if (appUser is null) throw new ApplicationException($"Unable to find user with id {userId}");

        if (_userManager.IsInRoleAsync(appUser, AdminRole).Result)
            throw new ApplicationException($"Unable to delete user with Admin role: {appUser.Email}");

        var result = await _userManager.DeleteAsync(appUser);

        if (!result.Succeeded)
            throw new ApplicationException($"Unable to delete user: {result.Errors.FirstOrDefault()?.Description}");
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        var appUser = await _userManager.FindByEmailAsync(email);
        return appUser is not null;
    }

    public async Task<bool> UserNameExistsAsync(string userName)
    {
        var appUser = await _userManager.FindByNameAsync(userName);
        return appUser is not null;
    }

    public async Task AddUserToRoleAsync(Guid userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is not null)
            await _userManager.AddToRoleAsync(user, roleName);
    }

    public async Task<IEnumerable<string>> GetUserRolesAsync(Guid userId)
    {
        var appUser = await _userManager.FindByIdAsync(userId.ToString());
        if (appUser != null)
        {
            return await _userManager.GetRolesAsync(appUser);
        }

        return new List<string>();
    }

    private async Task<User> CreateUser(User user, string? password = "")
    {
        var appUser = _mapper.Map<ApplicationUser>(user);

        IdentityResult result;

        if (!string.IsNullOrEmpty(password))
            result = await _userManager.CreateAsync(appUser, password);
        else
            result = await _userManager.CreateAsync(appUser);

        if (!result.Succeeded)
            throw new ApplicationException($"Unable to create user: {result.Errors.FirstOrDefault()?.Description}");

        return _mapper.Map<User>(appUser);
    }
}