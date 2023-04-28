using ErrorOr;

using MapsterMapper;

using MediatR;

using UserManager.Application.Common.DTOs.Role;
using UserManager.Application.Common.Interfaces.Repositories;
using UserManager.Domain.Common.Errors;

namespace UserManager.Application.Features.Roles.Queries.GetRole;

public class GetRoleQueryHandler : IRequestHandler<GetRoleQuery, ErrorOr<RoleDto>>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;

    public GetRoleQueryHandler(IRoleRepository roleRepository, IMapper mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    public async Task<ErrorOr<RoleDto>> Handle(GetRoleQuery query, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdAsync(new Guid(query.Id));

        if (role is null) return Errors.Role.RoleNotFound;

        return _mapper.Map<RoleDto>(role);
    }
}