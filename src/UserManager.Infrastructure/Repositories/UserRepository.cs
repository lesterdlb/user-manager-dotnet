using MapsterMapper;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using UserManager.Application.Common.DTOs.User;
using UserManager.Application.Common.Interfaces.Users;
using UserManager.Infrastructure.Identity;

namespace UserManager.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public UserRepository(UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<UserDto?> GetUserAsync(Guid userId)
    {
        var appUser = await _userManager.FindByIdAsync(userId.ToString());
        return appUser is null ? null : _mapper.Map<UserDto>(appUser);
    }

    public async Task<List<UserDto>> GetUsersAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        return _mapper.Map<List<UserDto>>(users);
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto user)
    {
        var appUser = _mapper.Map<ApplicationUser>(user);
        var result = await _userManager.CreateAsync(appUser, user.Password);

        if (!result.Succeeded)
        {
            throw new ApplicationException($"Unable to create user: {result.Errors.FirstOrDefault()?.Description}");
        }

        return _mapper.Map<UserDto>(appUser);
    }

    public Task<UserDto> UpdateUserAsync(UserDto user)
    {
        throw new NotImplementedException();
    }

    public Task DeleteUserAsync(Guid userId)
    {
        throw new NotImplementedException();
    }
}