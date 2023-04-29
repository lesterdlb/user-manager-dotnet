using System.ComponentModel.DataAnnotations;

namespace UserManager.MVC.Models.Roles;

public class CreateRoleViewModel
{
    [Required]
    public string Name { get; set; } = default!;
}