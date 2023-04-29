using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using UserManager.MVC.Contracts;
using UserManager.MVC.Models;
using UserManager.MVC.Models.Roles;

namespace UserManager.MVC.Controllers;

[Authorize(Roles = "Admin")]
public class RolesController : Controller
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<IActionResult> Index()
    {
        var model = await _roleService.GetRoles();
        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateRoleViewModel role)
    {
        if (!ModelState.IsValid) return View(role);

        try
        {
            var response = await _roleService.CreateRole(role);
            if (response.Success) return RedirectToAction(nameof(Index));

            ModelState.AddModelError(
                string.Empty,
                response.ValidationErrors.Count > 0
                    ? new ValidationErrorsModel(response.ValidationErrors).ToString()
                    : response.Message);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
        }

        return View(role);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var model = await _roleService.GetRole(id);

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var response = await _roleService.DeleteRole(id);
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
}