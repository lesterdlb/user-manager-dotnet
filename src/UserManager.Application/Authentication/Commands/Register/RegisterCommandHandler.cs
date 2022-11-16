using ErrorOr;
using MediatR;
using UserManager.Application.Common.Contracts.Authentication;
using UserManager.Application.Common.Interfaces.Authentication;

namespace UserManager.Application.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<RegisterResponse>>
{
    private readonly IIdentityService _identityService;

    public RegisterCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<ErrorOr<RegisterResponse>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        return await _identityService.RegisterUserAsync(command.Request);
    }
}