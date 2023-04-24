using ErrorOr;

using MediatR;

namespace UserManager.Application.Features.Roles.Commands.CreateRole;

public class CreateRoleCommand : BaseRoleCommand, IRequest<ErrorOr<Guid>>
{
}