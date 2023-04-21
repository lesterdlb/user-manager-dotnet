namespace UserManager.Application.Common.DTOs.User;

public class CreateUserDto
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public IEnumerable<string> Roles { get; set; } = null!;
}