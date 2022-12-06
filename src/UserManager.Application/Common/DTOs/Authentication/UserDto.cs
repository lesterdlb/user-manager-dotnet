namespace UserManager.Application.Common.DTOs.Authentication;

public class UserDto : BaseDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public Uri? ProfilePicture { get; set; }
    public string? Token { get; set; }
}