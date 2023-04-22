using FluentValidation;

using UserManager.Application.Common.Interfaces.Repositories;

namespace UserManager.Application.Features.Roles.Commands.CreateRole;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    private readonly IRoleRepository _roleRepository;

    public CreateRoleCommandValidator(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;

        RuleFor(c => c.Role.Name)
            .NotEmpty().WithMessage("Name is required");

        RuleFor(c => c)
            .MustAsync(RoleNameUnique)
            .WithMessage("A role with the same name already exists.");
    }

    private async Task<bool> RoleNameUnique(CreateRoleCommand command, CancellationToken token)
    {
        return !await _roleRepository.RoleExistsAsync(command.Role.Name);
    }
}