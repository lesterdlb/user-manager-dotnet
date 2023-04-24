using UserManager.Application.Common.Interfaces.Repositories;
using UserManager.Application.Features.Roles.Commands.CreateRole;

namespace UserManager.Application.Features.Roles.Commands.Validators;

public class CreateRoleCommandValidator : BaseRoleCommandValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator(IRoleRepository roleRepository) : base(roleRepository)
    {
    }
}