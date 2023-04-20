using MediatR;
using UserManager.Application.Common.DTOs.Authentication;

namespace UserManager.Application.Users.Queries.GetUsers;

public record GetUsersQuery() : IRequest<List<UserDto>>;