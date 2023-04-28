using ErrorOr;

using MapsterMapper;

using MediatR;

using UserManager.Application.Common.Interfaces.Repositories;
using UserManager.Domain.Entities;

namespace UserManager.Application.Features.Roles.Commands.CreateRole;

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, ErrorOr<Guid>>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;

    public CreateRoleCommandHandler(IRoleRepository roleRepository, IMapper mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    public async Task<ErrorOr<Guid>> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
    {
        var roleToCreate = _mapper.Map<Role>(command);

        var role = await _roleRepository.AddAsync(roleToCreate);

        return role.Id;
    }
}