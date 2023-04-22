using ErrorOr;

using MediatR;

using UserManager.Application.Common.Interfaces.Repositories;
using UserManager.Domain.Common.Errors;

namespace UserManager.Application.Features.Roles.Commands.DeleteRole;

public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, ErrorOr<Unit>>
{
    private readonly IRoleRepository _roleRepository;

    public DeleteRoleCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<ErrorOr<Unit>> Handle(DeleteRoleCommand command, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetRoleAsync(command.Id);

        if (role is null) return Errors.Role.RoleNotFound;

        await _roleRepository.DeleteRoleAsync(command.Id);

        return Unit.Value;
    }
}