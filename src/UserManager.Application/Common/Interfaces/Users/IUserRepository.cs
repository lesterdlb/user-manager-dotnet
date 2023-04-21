using UserManager.Application.Common.DTOs.User;

namespace UserManager.Application.Common.Interfaces.Users;

public interface IUserRepository
{
    Task<UserDto?> GetUserAsync(Guid userId);
    Task<List<UserDto>> GetUsersAsync();
    Task<UserDto> CreateUserAsync(CreateUserDto user);
    Task<UserDto> UpdateUserAsync(UserDto user);
    Task DeleteUserAsync(Guid userId);
}