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

        RuleFor(c => c.Name)
            .MustAsync(RoleNameUnique)
            .WithMessage("A role with the same name already exists.")
            .WithErrorCode("role.duplicate.name");
    }

    private async Task<bool> RoleNameUnique(string name, CancellationToken token)
    {
        return !await _roleRepository.RoleNameExistsAsync(name);
    }
}