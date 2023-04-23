using ErrorOr;

using MediatR;

namespace UserManager.Application.Features.Roles.Commands.UpdateRole;

public class UpdateRoleCommand : BaseRoleCommand, IRequest<ErrorOr<Unit>>
{
    public Guid Id { get; set; }
}