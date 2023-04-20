namespace UserManager.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(string userId, string userName, string email, IEnumerable<string> roles);
}