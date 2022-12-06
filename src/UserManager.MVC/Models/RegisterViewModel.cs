using System.ComponentModel.DataAnnotations;

namespace UserManager.MVC.Models;

public record RegisterViewModel
(
    [Required] string FirstName,
    [Required] string LastName,
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    string Email,
    [Required(ErrorMessage = "Username is required")]
    string UserName,
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    string Password
);