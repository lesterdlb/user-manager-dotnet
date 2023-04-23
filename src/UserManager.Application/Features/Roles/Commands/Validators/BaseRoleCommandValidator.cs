using FluentValidation;

using UserManager.Application.Common.Interfaces.Repositories;

namespace UserManager.Application.Features.Roles.Commands.Validators;

public abstract class BaseRoleCommandValidator<T> : AbstractValidator<T>
    where T : BaseRoleCommand
{
    private readonly IRoleRepository _roleRepository;

    protected BaseRoleCommandValidator(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;

        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Name is required");

        RuleFor(c => c)
            .MustAsync(RoleNameUnique)
            .WithMessage("A role with the same name already exists.");
    }

    private async Task<bool> RoleNameUnique(BaseRoleCommand command, CancellationToken token)
    {
        return !await _roleRepository.RoleExistsAsync(command.Name);
    }
}