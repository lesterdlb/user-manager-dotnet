using FluentValidation;

using UserManager.Application.Common.Interfaces.Repositories;
using UserManager.Application.Features.Roles.Commands.UpdateRole;

namespace UserManager.Application.Features.Roles.Commands.Validators;

public class UpdateRoleCommandValidator : BaseRoleCommandValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator(IRoleRepository roleRepository) : base(roleRepository)
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required");
    }
}