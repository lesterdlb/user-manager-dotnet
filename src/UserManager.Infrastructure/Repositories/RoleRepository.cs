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
        var roles = await _roleManager.Roles.Select(r => new RoleDto { Id = r.Id, Name = r.Name ?? string.Empty, })
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
            throw new ApplicationException($"Unable to create user: {result.Errors.FirstOrDefault()?.Description}");
        }

        return _mapper.Map<RoleDto>(appRole);
    }

    public Task<RoleDto> UpdateRoleAsync(RoleDto role)
    {
        throw new NotImplementedException();
    }

    public Task DeleteRoleAsync(Guid roleId)
    {
        throw new NotImplementedException();
    }
}