using ErrorOr;
using UserManager.Application.Common.Contracts.Authentication;

namespace UserManager.Application.Common.Interfaces.Authentication;

public interface IIdentityService
{
    Task<bool> UserByEmailExistsAsync(string email);
    Task<ErrorOr<RegisterResponse>> CreateUserAsync(RegisterRequest registerRequest, string password, string role);
    Task<ErrorOr<LoginResponse>> LoginUserAsync(LoginRequest request);
}