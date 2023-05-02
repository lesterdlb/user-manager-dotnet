using FluentValidation;

using UserManager.Application.Common.Interfaces.Repositories;

namespace UserManager.Application.Features.Users.Commands.Validators;

public abstract class BaseUserCommandValidator<T> : AbstractValidator<T>
    where T : BaseUserCommand
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;

    protected BaseUserCommandValidator(IUserRepository userRepository, IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;

        RuleFor(c => c.FirstName)
            .NotEmpty().WithMessage("First name is required.");

        RuleFor(c => c.LastName)
            .NotEmpty().WithMessage("Last name is required.");

        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is invalid.")
            .MustAsync(EmailUnique)
            .WithMessage("A user with the same email already exists.")
            .WithErrorCode("user.duplicate.email");

        RuleFor(c => c.UserName)
            .NotEmpty().WithMessage("Username is required.")
            .MustAsync(UserNameUnique)
            .WithMessage("A user with the same username already exists.")
            .WithErrorCode("user.duplicate.username");

        RuleFor(c => c.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

        RuleFor(c => c.RoleId)
            .NotEmpty().WithMessage("Role is required.")
            .MustAsync(RoleExists)
            .WithMessage("Role does not exist.")
            .WithErrorCode("user.role.notfound");
    }

    private async Task<bool> EmailUnique(string email, CancellationToken token)
        => !await _userRepository.EmailExistsAsync(email);

    private async Task<bool> UserNameUnique(string userName, CancellationToken token)
        => !await _userRepository.UserNameExistsAsync(userName);

    private async Task<bool> RoleExists(string roleId, CancellationToken token)
        => await _roleRepository.RoleIdExistsAsync(Guid.Parse(roleId));
}