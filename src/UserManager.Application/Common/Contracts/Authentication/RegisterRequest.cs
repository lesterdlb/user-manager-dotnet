namespace UserManager.Application.Common.Contracts.Authentication;

public record RegisterRequest(
    string FirstName,
    string LastName,
    string Email,
    string UserName,
    string Password);