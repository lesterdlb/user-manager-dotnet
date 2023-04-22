using MediatR;

using UserManager.Application.Common.DTOs.User;
using UserManager.Application.Common.Interfaces.Repositories;

namespace UserManager.Application.Features.Users.Queries.GetUsers;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserDto>>
{
    private readonly IUserRepository _userRepository;

    public GetUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return await _userRepository.GetUsersAsync();
    }
}