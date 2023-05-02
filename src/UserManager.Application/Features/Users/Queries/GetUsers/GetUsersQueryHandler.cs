using MapsterMapper;

using MediatR;

using UserManager.Application.Common.DTOs.User;
using UserManager.Application.Common.Interfaces.Repositories;

namespace UserManager.Application.Features.Users.Queries.GetUsers;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;

    public GetUsersQueryHandler(IUserRepository userRepository, IRoleRepository roleRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    public async Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllAsync();

        var usersDto = _mapper.Map<List<UserDto>>(users);

        foreach (var dto in usersDto)
        {
            var roles = await _userRepository.GetUserRolesNamesAsync(Guid.Parse(dto.Id));
            dto.Roles = await _roleRepository.GetRolesIdsAsync(roles);
        }

        return usersDto;
    }
}