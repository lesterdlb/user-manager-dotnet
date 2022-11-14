using MediatR;

namespace UserManager.Application.Authentication.Queries.Roles;

public record GetRolesQuery : IRequest<List<string>>;