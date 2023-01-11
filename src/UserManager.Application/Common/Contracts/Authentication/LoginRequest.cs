namespace UserManager.Application.Common.Contracts.Authentication;

public record LoginRequest(
    string Email,
    string Password);