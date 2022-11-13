using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using UserManager.Web.Models;
using UserManager.Web.ViewModels;

namespace UserManager.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserManagerController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserManagerController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleManager.Roles.Where(r => r.Name != Enums.Roles.Admin.ToString()).ToListAsync();
            var users = new List<ApplicationUser>();
            foreach (var role in roles)
            {
                users.AddRange(await _userManager.GetUsersInRoleAsync(role.Name));
            }

            return Ok(new { data = users });
        }

        [HttpPost]
        public async Task<IActionResult> Get(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is not null)
            {
                return Json(user);
            }

            return NotFound("User not found.");
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserViewModel input)
        {
            if (ModelState.IsValid)
            {
                MailAddress address = new(input.Email);
                string userName = address.User;
                var user = new ApplicationUser
                {
                    UserName = userName,
                    Email = input.Email,
                    FirstName = input.FirstName,
                    LastName = input.LastName,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, input.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Enums.Roles.Basic.ToString());
                    return Ok("User created!");
                }
            }

            return BadRequest("Error :(");
        }

        [HttpPut]
        public async Task<IActionResult> Edit(string Id, UserViewModel input)
        {
            ModelState.Remove("Password");
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(Id);

                user.Email = input.Email;
                user.FirstName = input.FirstName;
                user.LastName = input.LastName;

                await _userManager.UpdateAsync(user);

                return Ok("User Updated!");
            }

            return BadRequest("Error :(");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is not null)
            {
                await _userManager.DeleteAsync(user);
                return Ok("User deleted!");
            }

            return BadRequest("Error :(");
        }
    }
}
