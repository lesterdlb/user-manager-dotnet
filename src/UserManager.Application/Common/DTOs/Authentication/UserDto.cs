namespace UserManager.Application.Common.DTOs.Authentication;

public class UserDto : BaseDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public Uri? ProfilePicture { get; set; }
    public IEnumerable<string> Roles { get; set; } = null!;
    public string? Token { get; set; }
}