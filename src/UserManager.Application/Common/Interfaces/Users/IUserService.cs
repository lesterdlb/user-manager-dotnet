using UserManager.Application.Common.DTOs.Authentication;

namespace UserManager.Application.Common.Interfaces.Users;

public interface IUserService
{
    Task<UserDto?> GetUserAsync(string userId);
    Task<List<UserDto>> GetUsersAsync();
    Task<UserDto> CreateUserAsync(UserDto user);
    Task<UserDto> UpdateUserAsync(UserDto user);
    Task DeleteUserAsync(string userId);
}