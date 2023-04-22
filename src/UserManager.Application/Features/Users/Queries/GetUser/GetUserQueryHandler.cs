using ErrorOr;

using MediatR;

using UserManager.Application.Common.DTOs.User;
using UserManager.Application.Common.Interfaces.Repositories;
using UserManager.Domain.Common.Errors;

namespace UserManager.Application.Features.Users.Queries.GetUser;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, ErrorOr<UserDto>>
{
    private readonly IUserRepository _userRepository;

    public GetUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<UserDto>> Handle(GetUserQuery query, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserAsync(new Guid(query.Id));

        if (user is null) return Errors.User.UserNotFound;

        return user;
    }
}