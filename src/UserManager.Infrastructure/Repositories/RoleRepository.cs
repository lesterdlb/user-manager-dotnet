using MapsterMapper;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using UserManager.Application.Common.Interfaces.Repositories;
using UserManager.Domain.Entities;

namespace UserManager.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private const string AdminRole = "Admin";
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;

    public RoleRepository(RoleManager<IdentityRole> roleManager, IMapper mapper)
    {
        _roleManager = roleManager;
        _mapper = mapper;
    }

    public async Task<Role?> GetByIdAsync(Guid roleId)
    {
        var appRole = await _roleManager.FindByIdAsync(roleId.ToString());
        return (appRole is null || appRole.Name == AdminRole) ? null : _mapper.Map<Role>(appRole);
    }

    public async Task<IEnumerable<Role>> GetAllAsync()
    {
        var roles = await _roleManager.Roles
            .Where(r => r.Name != AdminRole)
            .OrderBy(r => r.Name)
            .ToListAsync();

        return _mapper.Map<List<Role>>(roles);
    }

    public async Task<Role> AddAsync(Role role)
    {
        var appRole = _mapper.Map<IdentityRole>(role);
        var result = await _roleManager.CreateAsync(appRole);

        if (!result.Succeeded)
            throw new ApplicationException($"Unable to create role: {result.Errors.FirstOrDefault()?.Description}");

        return _mapper.Map<Role>(appRole);
    }

    public async Task UpdateAsync(Role role)
    {
        var appRole = await _roleManager.FindByIdAsync(role.Id.ToString());

        if (appRole is null)
            throw new ApplicationException($"Unable to find role with id: {role.Id}");

        appRole.Name = role.Name;
        var result = await _roleManager.UpdateAsync(appRole);

        if (!result.Succeeded)
            throw new ApplicationException($"Unable to update role: {result.Errors.FirstOrDefault()?.Description}");
    }

    public async Task DeleteAsync(Guid roleId)
    {
        var appRole = await _roleManager.FindByIdAsync(roleId.ToString());

        if (appRole is null)
            throw new ApplicationException($"Unable to find role with id: {roleId}");

        await _roleManager.DeleteAsync(appRole);
    }

    public async Task<bool> RoleExistsAsync(string name)
        => await _roleManager.RoleExistsAsync(name);
}