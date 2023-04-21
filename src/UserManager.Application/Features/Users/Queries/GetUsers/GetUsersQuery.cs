using MediatR;

using UserManager.Application.Common.DTOs.User;

namespace UserManager.Application.Features.Users.Queries.GetUsers;

public record GetUsersQuery() : IRequest<List<UserDto>>;