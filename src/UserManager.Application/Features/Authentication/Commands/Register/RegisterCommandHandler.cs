using ErrorOr;

using MediatR;

using UserManager.Application.Common.Contracts.Authentication;
using UserManager.Application.Common.Interfaces.Authentication;
using UserManager.Domain.Common.Errors;

namespace UserManager.Application.Features.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<RegisterResponse>>
{
    private readonly IIdentityService _identityService;

    public RegisterCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<ErrorOr<RegisterResponse>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var userExists = await _identityService.UserByEmailExistsAsync(command.Request.Email);

        if (userExists) return Errors.User.DuplicateEmail;

        var user = await _identityService.CreateUserAsync(
            command.Request, command.Request.Password, Domain.Enums.Roles.User.ToString());

        if (user.IsError) return user.FirstError;

        return user.Value;
    }
}