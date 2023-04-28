using MediatR;

using UserManager.Application.Common.DTOs.Role;

namespace UserManager.Application.Features.Roles.Queries.GetRoles;

public record GetRolesQuery : IRequest<List<RoleDto>>;