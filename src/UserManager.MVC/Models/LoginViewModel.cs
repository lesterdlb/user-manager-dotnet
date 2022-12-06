using System.ComponentModel.DataAnnotations;

namespace UserManager.MVC.Models;

public record LoginViewModel
(
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    string Email,
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    string Password,
    string? ReturnUrl
);