using ErrorOr;

using MediatR;

using UserManager.Application.Common.DTOs.Role;

namespace UserManager.Application.Features.Roles.Queries.GetRole;

public record GetRoleQuery(string Id) : IRequest<ErrorOr<RoleDto>>;