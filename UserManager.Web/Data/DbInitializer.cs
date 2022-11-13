using Microsoft.AspNetCore.Identity;
using UserManager.Web.Models;

namespace UserManager.Web.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            // Create Roles
            if (!_roleManager.RoleExistsAsync(Enums.Roles.Admin.ToString()).Result)
            {
                _roleManager.CreateAsync(new IdentityRole(Enums.Roles.Admin.ToString())).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(Enums.Roles.Basic.ToString())).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(Enums.Roles.Student.ToString())).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(Enums.Roles.Teacher.ToString())).GetAwaiter().GetResult();
            }

            // Create dafault Admin User
            var defaultUser = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@gmail.com",
                FirstName = "Admin",
                LastName = "Admin",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            if (_userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = _userManager.FindByEmailAsync(defaultUser.Email).GetAwaiter().GetResult();
                if (user == null)
                {
                    _userManager.CreateAsync(defaultUser, "123qwe").GetAwaiter().GetResult();
                    _userManager.AddToRoleAsync(defaultUser, Enums.Roles.Admin.ToString()).GetAwaiter().GetResult();
                }
            }
        }
    }
}
