using UserManager.MVC.Models;

namespace UserManager.MVC.Contracts;

public interface IAuthenticationService
{
    Task<bool> Login(string email, string password);
    Task<bool> Register(RegisterViewModel register);
    Task Logout();
}