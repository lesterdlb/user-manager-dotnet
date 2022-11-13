using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UserManager.Application.Common.Interfaces.Services;
using UserManager.Infrastructure.Authentication;

namespace UserManager.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly JwtSettings _jwtSettings;

    public IdentityService(
        IOptions<JwtSettings> jwtOptions, 

        RoleManager<IdentityRole<Guid>> roleManager)
    {
        _roleManager = roleManager;
        _jwtSettings = jwtOptions.Value;
    }

    public async Task<List<string>> GetRoles()
    {
        var roles = await _roleManager.Roles.ToListAsync();
        return roles.Select(x => x.Name).ToList();
    }
}