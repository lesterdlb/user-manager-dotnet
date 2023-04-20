using MediatR;

using UserManager.Application.Common.DTOs.Authentication;
using UserManager.Application.Common.Interfaces.Users;

namespace UserManager.Application.Users.Queries.GetUsers;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserDto>>
{
    private readonly IUserService _userService;

    public GetUsersQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return await _userService.GetUsersAsync();
    }
}