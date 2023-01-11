namespace UserManager.Application.Common.Contracts.Authentication;

public record LoginResponse(
    string Id,
    string FirstName,
    string LastName,
    string Email,
    string Token);