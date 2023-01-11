using ErrorOr;
using MapsterMapper;
using MediatR;
using UserManager.Application.Common.Contracts.Authentication;
using UserManager.Application.Common.Interfaces.Authentication;
using UserManager.Domain.Common.Errors;

namespace UserManager.Application.Authentication.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<LoginResponse>>
{
    private readonly IIdentityService _identityService;
    private readonly IMapper _mapper;

    public LoginQueryHandler(IIdentityService identityService, IMapper mapper)
    {
        _identityService = identityService;
        _mapper = mapper;
    }

    public async Task<ErrorOr<LoginResponse>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        var userExists = await _identityService.UserByEmailExistsAsync(query.Request.Email);

        if (!userExists)
            return Errors.User.UserNotFound;

        var user = await _identityService.LoginUserAsync(query.Request);

        if (user is null)
            return Errors.Authentication.InvalidCredentials;

        return _mapper.Map<LoginResponse>(user);
    }
}