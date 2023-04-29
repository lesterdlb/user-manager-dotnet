using Microsoft.AspNetCore.Mvc;

using UserManager.MVC.Contracts;
using UserManager.MVC.Models;

namespace UserManager.MVC.Controllers;

public class AuthenticationController : Controller
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public IActionResult Index()
    {
        return NoContent();
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel login, string? returnUrl = null)
    {
        if (ModelState.IsValid)
        {
            var isSignedIn = await _authenticationService.Login(login.Email, login.Password);
            if (isSignedIn)
                return LocalRedirect(returnUrl ?? Url.Content("~/"));
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt");
        return View(login);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Logout(string? returnUrl)
    {
        await _authenticationService.Logout();
        return LocalRedirect(returnUrl ?? Url.Content("~/"));
    }
}