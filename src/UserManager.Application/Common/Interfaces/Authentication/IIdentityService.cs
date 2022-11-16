using ErrorOr;
using UserManager.Application.Common.Contracts.Authentication;

namespace UserManager.Application.Common.Interfaces.Authentication;

public interface IIdentityService
{
    Task<ErrorOr<RegisterResponse>> RegisterUserAsync(RegisterRequest request);
    Task<ErrorOr<LoginResponse>> LoginUserAsync(LoginRequest request);
    Task<List<string>> GetRoles();
}