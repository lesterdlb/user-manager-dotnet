using ErrorOr;

using MapsterMapper;

using MediatR;

using UserManager.Application.Common.DTOs.User;
using UserManager.Application.Common.Interfaces.Repositories;
using UserManager.Domain.Common.Errors;

namespace UserManager.Application.Features.Users.Queries.GetUser;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, ErrorOr<UserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUserQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<ErrorOr<UserDto>> Handle(GetUserQuery query, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(new Guid(query.Id));

        if (user is null) return Errors.User.UserNotFound;

        var userDto = _mapper.Map<UserDto>(user);

        userDto.Roles = await _userRepository.GetUserRolesAsync(user.Id);

        return _mapper.Map<UserDto>(user);
    }
}