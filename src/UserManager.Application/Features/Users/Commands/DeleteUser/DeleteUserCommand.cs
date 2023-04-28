using ErrorOr;

using MediatR;

namespace UserManager.Application.Features.Users.Commands.DeleteUser;

public record DeleteUserCommand(Guid Id) : IRequest<ErrorOr<Unit>>;