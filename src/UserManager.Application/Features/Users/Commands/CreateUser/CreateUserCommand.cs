using ErrorOr;

using MediatR;

namespace UserManager.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommand : BaseUserCommand, IRequest<ErrorOr<Guid>>
{
}