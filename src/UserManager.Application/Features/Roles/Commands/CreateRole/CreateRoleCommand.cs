using ErrorOr;

using MediatR;

using UserManager.Application.Common.DTOs.Role;

namespace UserManager.Application.Features.Roles.Commands.CreateRole;

public record CreateRoleCommand(CreateRoleDto Role) : IRequest<ErrorOr<Guid>>;