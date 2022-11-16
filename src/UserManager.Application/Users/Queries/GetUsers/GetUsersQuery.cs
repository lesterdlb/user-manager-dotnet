using MediatR;
using UserManager.Domain.Common.DTOs.User;

namespace UserManager.Application.Users.Queries.GetUsers;

public record GetUsersQuery() : IRequest<List<UserDto>>;