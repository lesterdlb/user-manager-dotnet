using UserManager.Application.Common.Interfaces.Repositories;
using UserManager.Application.Features.Users.Commands.CreateUser;

namespace UserManager.Application.Features.Users.Commands.Validators;

public class CreateUserCommandValidator : BaseUserCommandValidator<CreateUserCommand>
{
    public CreateUserCommandValidator(IUserRepository userRepository, IRoleRepository roleRepository)
        : base(userRepository, roleRepository)
    {
    }
}