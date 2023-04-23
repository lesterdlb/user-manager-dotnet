using ErrorOr;

using MediatR;

using UserManager.Application.Common.Interfaces.Repositories;
using UserManager.Domain.Common.Errors;

namespace UserManager.Application.Features.Roles.Commands.UpdateRole;

public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, ErrorOr<Unit>>
{
    private readonly IRoleRepository _roleRepository;

    public UpdateRoleCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<ErrorOr<Unit>> Handle(UpdateRoleCommand command, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetRoleAsync(command.Id);

        if (role is null) return Errors.Role.RoleNotFound;

        role.Name = command.Name;
        await _roleRepository.UpdateRoleAsync(role);

        return Unit.Value;
    }
}