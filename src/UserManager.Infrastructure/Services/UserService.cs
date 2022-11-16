using MapsterMapper;
using UserManager.Application.Common.Interfaces.Users;
using UserManager.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManager.Domain.Common.DTOs.User;

namespace UserManager.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public UserService(UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<UserDto?> GetUserAsync(string userId)
    {
        var appUser = await _userManager.FindByIdAsync(userId);
        return _mapper.Map<UserDto>(appUser);
    }

    public async Task<List<UserDto>> GetUsersAsync()
    {
        var appUsers = await _userManager.Users.ToListAsync();
        return _mapper.Map<List<UserDto>>(appUsers);
    }

    public Task<UserDto> CreateUserAsync(UserDto user)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> UpdateUserAsync(UserDto user)
    {
        throw new NotImplementedException();
    }

    public Task DeleteUserAsync(string userId)
    {
        throw new NotImplementedException();
    }
}