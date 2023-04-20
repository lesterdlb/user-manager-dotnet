using ErrorOr;

using MapsterMapper;

using MediatR;

using UserManager.Application.Common.Contracts.Authentication;
using UserManager.Application.Common.Interfaces.Authentication;
using UserManager.Domain.Common.Errors;

namespace UserManager.Application.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<RegisterResponse>>
{
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;

    public RegisterCommandHandler(IIdentityService identityService, IMapper mapper)
    {
        _identityService = identityService;
        _mapper = mapper;
    }

    public async Task<ErrorOr<RegisterResponse>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var userExists = await _identityService.UserByEmailExistsAsync(command.Request.Email);
        var roleExists = await _identityService.RoleExistsAsync("User");

        if (userExists) return Errors.User.DuplicateEmail;

        if (!roleExists) return Errors.Role.RoleNotFound;

        var user = await _identityService.CreateUserAsync(
            command.Request, command.Request.Password, "User");

        if (user.IsError) return user.FirstError;

        return new RegisterResponse(user.Value.UserId);
    }
}