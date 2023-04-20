namespace UserManager.Application.Common.DTOs;

public record IdentityUserDto(string Id, string UserName, string Email, IEnumerable<string> Roles);