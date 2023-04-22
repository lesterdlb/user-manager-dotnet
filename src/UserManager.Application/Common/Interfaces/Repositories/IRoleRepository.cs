using UserManager.Application.Common.DTOs.Role;

namespace UserManager.Application.Common.Interfaces.Repositories;

public interface IRoleRepository
{
    Task<RoleDto?> GetRoleAsync(Guid roleId);
    Task<List<RoleDto>> GetRolesAsync();
    Task<bool> RoleExistsAsync(string name);
    Task<RoleDto> CreateRoleAsync(CreateRoleDto role);
    Task<RoleDto> UpdateRoleAsync(RoleDto role);
    Task DeleteRoleAsync(Guid userId);
}