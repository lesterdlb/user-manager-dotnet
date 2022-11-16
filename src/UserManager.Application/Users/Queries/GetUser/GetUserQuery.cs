using MediatR;
using UserManager.Domain.Common.DTOs.User;
using ErrorOr;

namespace UserManager.Application.Users.Queries.GetUser;

public record GetUserQuery(string Id) : IRequest<ErrorOr<UserDto>>;