using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using UserManager.MVC.Contracts;
using UserManager.MVC.Models.Roles;
using UserManager.MVC.Models.Users;

namespace UserManager.MVC.Controllers;

[Authorize(Roles = "Admin")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    private readonly IRoleService _roleService;

    public UsersController(IUserService userService, IRoleService roleService)
    {
        _userService = userService;
        _roleService = roleService;
    }

    public async Task<IActionResult> Index()
    {
        var model = await _userService.GetUsers();
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new CreateUserViewModel { RolesList = await GetRoles() };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.RolesList = await GetRoles();
            return View(model);
        }

        try
        {
            var response = await _userService.CreateUser(model);
            if (response.Success) return RedirectToAction(nameof(Index));

            if (response.ValidationErrors.Count > 0)
                response.ValidationErrors.ForEach(x => ModelState.AddModelError(string.Empty, x));
            else
                ModelState.AddModelError(string.Empty, response.Message);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
        }

        model.RolesList = await GetRoles();
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var model = await _userService.GetUser(id);

        return NoContent();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var response = await _userService.DeleteUser(id);
            if (response.Success)
            {
                return RedirectToAction(nameof(Index));
            }

            response.ValidationErrors.ForEach(x => ModelState.AddModelError(string.Empty, x));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
        }

        return BadRequest();
    }

    private async Task<List<RoleViewModel>> GetRoles()
    {
        return await _roleService.GetRoles();
    }
}