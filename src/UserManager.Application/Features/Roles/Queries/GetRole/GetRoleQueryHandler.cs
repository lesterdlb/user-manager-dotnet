using ErrorOr;

using MediatR;

using UserManager.Application.Common.DTOs.Role;
using UserManager.Application.Common.Interfaces.Repositories;
using UserManager.Domain.Common.Errors;

namespace UserManager.Application.Features.Roles.Queries.GetRole;

public class GetRoleQueryHandler : IRequestHandler<GetRoleQuery, ErrorOr<RoleDto>>
{
    private readonly IRoleRepository _roleRepository;

    public GetRoleQueryHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<ErrorOr<RoleDto>> Handle(GetRoleQuery query, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetRoleAsync(new Guid(query.Id));

        if (role is null) return Errors.Role.RoleNotFound;

        return role;
    }
}