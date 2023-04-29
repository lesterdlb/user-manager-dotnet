using UserManager.MVC.Models.Roles;
using UserManager.MVC.Services.Base;

namespace UserManager.MVC.Contracts;

public interface IRoleService
{
    Task<List<RoleViewModel>> GetRoles();
    Task<RoleViewModel> GetRole(Guid id);
    Task<Response<Guid>> CreateRole(CreateRoleViewModel role);
    Task<Response<Guid>> UpdateRole(RoleViewModel role);
    Task<Response<Guid>> DeleteRole(Guid id);
}