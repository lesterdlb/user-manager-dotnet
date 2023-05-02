using System.ComponentModel.DataAnnotations;

using UserManager.MVC.Models.Roles;

namespace UserManager.MVC.Models.Users;

public class CreateUserViewModel
{
    [Required]
    public string FirstName { get; set; } = default!;

    [Required]
    public string LastName { get; set; } = default!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;

    [Required]
    public string UserName { get; set; } = default!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;

    [Required]
    public Guid RoleId { get; set; }

    public List<RoleViewModel> RolesList { get; set; } = default!;
}