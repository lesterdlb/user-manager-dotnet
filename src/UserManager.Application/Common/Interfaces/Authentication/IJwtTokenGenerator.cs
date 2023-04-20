using UserManager.Application.Common.DTOs;

namespace UserManager.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(IdentityUserDto user);
}