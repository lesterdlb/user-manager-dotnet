using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserManager.Infrastructure.Identity;

namespace UserManager.Infrastructure.Persistence;

public class ApplicationDbContextInitializer
{
    private readonly ILogger<ApplicationDbContextInitializer> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContextInitializer(
        ILogger<ApplicationDbContextInitializer> logger,
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        // Default roles
        var adminRole = new IdentityRole("Admin");
        var userRole = new IdentityRole("User");

        if (_roleManager.Roles.All(r =>
                r.Name != adminRole.Name && r.Name != userRole.Name))
        {
            await _roleManager.CreateAsync(adminRole);
            await _roleManager.CreateAsync(userRole);
        }
        
        // Default users
        var adminUser = new ApplicationUser
        {
            UserName = "admin",
            Email = "admin@localhost",
            EmailConfirmed = true,
            FirstName = "Admin",
            LastName = "User",
            PhoneNumber = "1234567890",
            PhoneNumberConfirmed = true
        };
        
        var user = new ApplicationUser
        {
            UserName = "user",
            Email = "user@localhost",
            EmailConfirmed = true,
            FirstName = "Normal",
            LastName = "User",
            PhoneNumber = "1234567890",
            PhoneNumberConfirmed = true
        };

        if (_userManager.Users.All(u =>
                u.UserName != adminUser.UserName && u.UserName != user.UserName))
        {
            await _userManager.CreateAsync(adminUser, "123qwe");
            await _userManager.CreateAsync(user, "123qwe");
        }
    }
}