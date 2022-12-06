using ErrorOr;
using UserManager.Application.Common.Contracts.Authentication;
using UserManager.Application.Common.DTOs.Authentication;

namespace UserManager.Application.Common.Interfaces.Authentication;

public interface IIdentityService
{
    Task<bool> UserByEmailExistsAsync(string email);
    Task<bool> RoleExistsAsync(string name);
    Task<UserDto?> CreateUserAsync(RegisterRequest registerRequest, string password, string role);
    Task<UserDto?> LoginUserAsync(LoginRequest request);
    Task<List<string>> GetRoles();
}