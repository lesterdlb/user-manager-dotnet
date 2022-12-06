using MediatR;
using UserManager.Application.Common.Interfaces.Users;
using ErrorOr;
using UserManager.Application.Common.DTOs.Authentication;
using UserManager.Domain.Common.Errors;

namespace UserManager.Application.Users.Queries.GetUser;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, ErrorOr<UserDto>>
{
    private readonly IUserService _userService;

    public GetUserQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<ErrorOr<UserDto>> Handle(GetUserQuery query, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserAsync(query.Id);

        if (user is null)
            return Errors.User.UserNotFound;

        return user;
    }
};