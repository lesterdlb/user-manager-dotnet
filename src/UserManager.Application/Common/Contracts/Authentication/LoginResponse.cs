namespace UserManager.Application.Common.Contracts.Authentication;

public record LoginResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Token
);