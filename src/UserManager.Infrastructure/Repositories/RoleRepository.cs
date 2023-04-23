using MapsterMapper;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using UserManager.Application.Common.DTOs.Role;
using UserManager.Application.Common.Interfaces.Repositories;

namespace UserManager.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;

    public RoleRepository(RoleManager<IdentityRole> roleManager, IMapper mapper)
    {
        _roleManager = roleManager;
        _mapper = mapper;
    }

    public async Task<RoleDto?> GetRoleAsync(Guid roleId)
    {
        var appRole = await _roleManager.FindByIdAsync(roleId.ToString());
        return appRole is null ? null : _mapper.Map<RoleDto>(appRole);
    }

    public async Task<List<RoleDto>> GetRolesAsync()
    {
        var roles = await _roleManager.Roles
            .Select(r => new RoleDto { Id = r.Id, Name = r.Name ?? string.Empty, })
            .OrderBy(r => r.Name)
            .ToListAsync();

        return _mapper.Map<List<RoleDto>>(roles);
    }

    public async Task<bool> RoleExistsAsync(string name)
        => await _roleManager.RoleExistsAsync(name);

    public async Task<RoleDto> CreateRoleAsync(CreateRoleDto role)
    {
        var appRole = _mapper.Map<IdentityRole>(role);
        var result = await _roleManager.CreateAsync(appRole);

        if (!result.Succeeded)
        {
            throw new ApplicationException($"Unable to create role: {result.Errors.FirstOrDefault()?.Description}");
        }

        return _mapper.Map<RoleDto>(appRole);
    }

    public async Task<RoleDto> UpdateRoleAsync(RoleDto role)
    {
        var appRole = await _roleManager.FindByIdAsync(role.Id);

        if (appRole is null)
        {
            throw new ApplicationException($"Unable to find role with id: {role.Id}");
        }

        appRole.Name = role.Name;
        var result = await _roleManager.UpdateAsync(appRole);

        if (!result.Succeeded)
        {
            throw new ApplicationException($"Unable to update role: {result.Errors.FirstOrDefault()?.Description}");
        }

        return _mapper.Map<RoleDto>(appRole);
    }

    public async Task DeleteRoleAsync(Guid roleId)
    {
        var appRole = await _roleManager.FindByIdAsync(roleId.ToString());
        await _roleManager.DeleteAsync(appRole!);
    }
}