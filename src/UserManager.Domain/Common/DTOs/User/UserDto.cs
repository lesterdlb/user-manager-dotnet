using UserManager.Domain.Common.DTOs.Common;

namespace UserManager.Domain.Common.DTOs.User;

public class UserDto : BaseDto
{
    public string FirstName { get; set; }  = null!;
    public string LastName { get; set; }  = null!;
    public string Email { get; set; } = null!;
    public Uri? ProfilePicture { get; set; }
}