﻿using ErrorOr;

using Mapster;

using MapsterMapper;

using Moq;

using Shouldly;

using UserManager.Application.Common.Interfaces.Repositories;
using UserManager.Application.Features.Roles.Commands.CreateRole;
using UserManager.Application.UnitTests.Mocks;

namespace UserManager.Application.UnitTests.Roles.Commands;

public class CreateRoleCommandHandlerTests
{
    private readonly Mock<IRoleRepository> _roleRepository;
    private readonly IMapper _mapper;

    public CreateRoleCommandHandlerTests()
    {
        _roleRepository = RepositoryMocks.GetRoleRepository();
        var config = new TypeAdapterConfig();
        _mapper = new Mapper(config);
    }

    [Fact]
    public async Task CreateRoleTest()
    {
        var handler = new CreateRoleCommandHandler(_roleRepository.Object, _mapper);
        var command = new CreateRoleCommand { Name = "Test" };

        var result = await handler.Handle(command, CancellationToken.None);

        result.ShouldBeOfType<ErrorOr<Guid>>();
        result.Value.ShouldBe(Guid.Parse(RepositoryMocks.GetNewRoleId()));
    }
}