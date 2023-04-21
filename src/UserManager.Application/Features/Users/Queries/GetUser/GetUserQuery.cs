using ErrorOr;

using MediatR;

using UserManager.Application.Common.DTOs.User;

namespace UserManager.Application.Features.Users.Queries.GetUser;

public record GetUserQuery(string Id) : IRequest<ErrorOr<UserDto>>;