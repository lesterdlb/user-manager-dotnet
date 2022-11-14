using MediatR;
using UserManager.Application.Common.Interfaces.Services;

namespace UserManager.Application.Authentication.Queries.Roles;

public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, List<string>>
{
    private readonly IIdentityService _identityService;

    public GetRolesQueryHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<List<string>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        return await _identityService.GetRoles();
    }
}