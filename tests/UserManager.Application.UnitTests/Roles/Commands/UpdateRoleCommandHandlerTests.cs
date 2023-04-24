using MediatR;

using Moq;

using Shouldly;

using UserManager.Application.Common.DTOs.Role;
using UserManager.Application.Common.Interfaces.Repositories;
using UserManager.Application.Features.Roles.Commands.UpdateRole;
using UserManager.Application.UnitTests.Mocks;
using UserManager.Domain.Common.Errors;

namespace UserManager.Application.UnitTests.Roles.Commands;

public class UpdateRoleCommandHandlerTests
{
    private readonly Mock<IRoleRepository> _roleRepository;

    public UpdateRoleCommandHandlerTests()
    {
        _roleRepository = RepositoryMocks.GetRoleRepository();
    }

    [Fact]
    public async Task UpdateRoleTest()
    {
        var handler = new UpdateRoleCommandHandler(_roleRepository.Object);
        var command = new UpdateRoleCommand { Id = Guid.Parse(RepositoryMocks.GetUserRoleId()), Name = "Test" };

        var result = await handler.Handle(command, CancellationToken.None);

        result.Value.ShouldBe(Unit.Value);
        _roleRepository.Verify(x => x.GetRoleAsync(Guid.Parse(RepositoryMocks.GetUserRoleId())), Times.Once);
        _roleRepository.Verify(x => x.UpdateRoleAsync(It.IsAny<RoleDto>()), Times.Once);
    }

    [Fact]
    public async Task UpdateRoleTest_InvalidId()
    {
        var handler = new UpdateRoleCommandHandler(_roleRepository.Object);
        var command = new UpdateRoleCommand { Id = Guid.Empty, Name = "Test" };

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsError.ShouldBe(true);
        result.FirstError.ShouldBeOfType(Errors.Role.RoleNotFound.GetType());
        _roleRepository.Verify(x => x.GetRoleAsync(Guid.Empty), Times.Once);
    }
}