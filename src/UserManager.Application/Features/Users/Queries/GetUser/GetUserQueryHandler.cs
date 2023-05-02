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
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;

    public GetUserQueryHandler(IUserRepository userRepository, IRoleRepository roleRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    public async Task<ErrorOr<UserDto>> Handle(GetUserQuery query, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(new Guid(query.Id));

        if (user is null) return Errors.User.UserNotFound;

        var userDto = _mapper.Map<UserDto>(user);

        var roles = await _userRepository.GetUserRolesNamesAsync(user.Id);
        userDto.Roles = await _roleRepository.GetRolesIdsAsync(roles);

        return userDto;
    }
}