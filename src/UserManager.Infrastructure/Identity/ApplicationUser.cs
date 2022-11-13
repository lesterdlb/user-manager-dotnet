using Microsoft.AspNetCore.Identity;

namespace UserManager.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Uri? ProfilePicture { get; set; }
}