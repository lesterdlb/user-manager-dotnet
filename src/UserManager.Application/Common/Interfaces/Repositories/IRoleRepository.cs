using UserManager.Domain.Entities;

namespace UserManager.Application.Common.Interfaces.Repositories;

public interface IRoleRepository : IAsyncRepository<Role>
{
    Task<bool> RoleIdExistsAsync(Guid id);
    Task<bool> RoleNameExistsAsync(string name);
    Task<IEnumerable<string>> GetRolesIdsAsync(IEnumerable<string> roleNames);
    Task<string> GetRoleNameByIdAsync(Guid roleId);
}