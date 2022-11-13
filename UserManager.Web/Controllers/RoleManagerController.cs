using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UserManager.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleManagerController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleManagerController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var roles = await _roleManager.Roles.Where(x => x.Name != Enums.Roles.Admin.ToString()).ToListAsync();
            return Ok(new { data = roles });
        }

        [HttpPost]
        public async Task<IActionResult> Add(string name)
        {
            if (name is not null)
            {
                await _roleManager.CreateAsync(new IdentityRole(name.Trim()));
                return Ok("Role created!");
            }

            return BadRequest("Error :(");
        }

        [HttpPut]
        public async Task<IActionResult> Edit(string id, string name)
        {
            if (name is not null)
            {
                var role = await _roleManager.FindByIdAsync(id);
                role.Name = name;
                await _roleManager.UpdateAsync(role);
                return Ok("Role updated!");
            }

            return BadRequest("Error :(");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role is not null)
            {
                await _roleManager.DeleteAsync(role);
                return Ok("Role deleted!");

            }

            return BadRequest("Error :(");
        }
    }

}
