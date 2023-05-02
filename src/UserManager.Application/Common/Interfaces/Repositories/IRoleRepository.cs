using UserManager.Domain.Entities;

namespace UserManager.Application.Common.Interfaces.Repositories;

public interface IRoleRepository : IAsyncRepository<Role>
{
    Task<bool> RoleExistsAsync(string name);
    Task<IEnumerable<string>> GetRolesIdsAsync(IEnumerable<string> roleNames);
}