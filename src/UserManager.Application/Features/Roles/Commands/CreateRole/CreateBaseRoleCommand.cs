using ErrorOr;

using MediatR;

namespace UserManager.Application.Features.Roles.Commands.CreateRole;

public class CreateBaseRoleCommand : BaseRoleCommand, IRequest<ErrorOr<Guid>>
{
}