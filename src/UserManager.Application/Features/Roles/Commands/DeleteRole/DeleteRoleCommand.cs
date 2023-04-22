using ErrorOr;

using MediatR;

namespace UserManager.Application.Features.Roles.Commands.DeleteRole;

public record DeleteRoleCommand(Guid Id) : IRequest<ErrorOr<Unit>>;