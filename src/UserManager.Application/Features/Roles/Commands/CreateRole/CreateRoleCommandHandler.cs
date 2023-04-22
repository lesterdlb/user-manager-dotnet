using ErrorOr;

using MediatR;

using UserManager.Application.Common.Interfaces.Repositories;

namespace UserManager.Application.Features.Roles.Commands.CreateRole;

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, ErrorOr<Guid>>
{
    private readonly IRoleRepository _roleRepository;

    public CreateRoleCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<ErrorOr<Guid>> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.CreateRoleAsync(command.Role);

        return new Guid(role.Id);
    }
}