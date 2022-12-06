using MediatR;
using ErrorOr;
using UserManager.Application.Common.DTOs.Authentication;

namespace UserManager.Application.Users.Queries.GetUser;

public record GetUserQuery(string Id) : IRequest<ErrorOr<UserDto>>;