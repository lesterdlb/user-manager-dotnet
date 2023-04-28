using ErrorOr;

using MapsterMapper;

using MediatR;

using UserManager.Application.Common.Interfaces.Repositories;
using UserManager.Domain.Entities;

namespace UserManager.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ErrorOr<Guid>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<ErrorOr<Guid>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var userToCreate = _mapper.Map<User>(command);

        var user = await _userRepository.AddUserWithPasswordAsync(userToCreate, command.Password);

        await _userRepository.AddUserToRoleAsync(user.Id, command.Role);

        return user.Id;
    }
}