using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using UserManager.MVC.Contracts;
using UserManager.MVC.Models;
using UserManager.MVC.Services.Base;
using IAuthenticationService = UserManager.MVC.Contracts.IAuthenticationService;

namespace UserManager.MVC.Services;

public class AuthenticationService : BaseHttpService, IAuthenticationService
{
    private readonly ILocalStorageService _localStorageService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtSecurityTokenHandler _tokenHandler;
    private readonly ILogger<AuthenticationService> _logger;

    public AuthenticationService(
        IClient client,
        ILocalStorageService localStorageService,
        IHttpContextAccessor httpContextAccessor,
        ILogger<AuthenticationService> logger) : base(client, localStorageService)
    {
        _localStorageService = localStorageService;
        _httpContextAccessor = httpContextAccessor;
        _tokenHandler = new JwtSecurityTokenHandler();
        _logger = logger;
    }

    public async Task<bool> Login(string email, string password)
    {
        try
        {
            var loginRequest = new LoginRequest { Email = email, Password = password };
            var loginResponse = await Client.LoginAsync(loginRequest);

            if (loginResponse.Token != string.Empty)
            {
                var tokenContent = _tokenHandler.ReadJwtToken(loginResponse.Token);
                var claims = ParseClaims(tokenContent);
                var user = new ClaimsPrincipal(new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme));

                if (_httpContextAccessor.HttpContext is null) return false;

                await _httpContextAccessor.HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme, user);
                _localStorageService.SetStorageValue("token", loginResponse.Token);

                return true;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while logging in");
            return false;
        }

        return false;
    }

    public Task<bool> Register(RegisterViewModel register)
    {
        throw new NotImplementedException();
    }

    public async Task Logout()
    {
        if (_httpContextAccessor.HttpContext != null)
        {
            _localStorageService.ClearStorage(new List<string> { "token" });
            await _httpContextAccessor.HttpContext
                .SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }

    private static IEnumerable<Claim> ParseClaims(JwtSecurityToken tokenContent)
    {
        var claims = tokenContent.Claims.ToList();
        claims.Add(new Claim(ClaimTypes.Name, tokenContent.Subject));
        return claims;
    }
}