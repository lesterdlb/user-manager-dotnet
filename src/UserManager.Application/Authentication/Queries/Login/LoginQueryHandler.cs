using ErrorOr;
using MediatR;
using UserManager.Application.Common.Contracts.Authentication;
using UserManager.Application.Common.Interfaces.Authentication;

namespace UserManager.Application.Authentication.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<LoginResponse>>
{
    private readonly IIdentityService _identityService;

    public LoginQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<ErrorOr<LoginResponse>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        return await _identityService.LoginUserAsync(query.Request);
    }
};