using MediatR;

using Moq;

using Shouldly;

using UserManager.Application.Common.Interfaces.Repositories;
using UserManager.Application.Features.Roles.Commands.DeleteRole;
using UserManager.Application.UnitTests.Mocks;
using UserManager.Domain.Common.Errors;

namespace UserManager.Application.UnitTests.Roles.Commands;

public class DeleteRoleCommandHandlerTests
{
    private readonly Mock<IRoleRepository> _roleRepository;

    public DeleteRoleCommandHandlerTests()
    {
        _roleRepository = RepositoryMocks.GetRoleRepository();
    }

    [Fact]
    public async Task DeleteRoleTest()
    {
        var handler = new DeleteRoleCommandHandler(_roleRepository.Object);
        var command = new DeleteRoleCommand(Guid.Parse(RepositoryMocks.GetUserRoleId()));

        var result = await handler.Handle(command, CancellationToken.None);

        result.Value.ShouldBe(Unit.Value);
        _roleRepository.Verify(x => x.DeleteAsync(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task DeleteRoleTest_InvalidId()
    {
        var handler = new DeleteRoleCommandHandler(_roleRepository.Object);
        var command = new DeleteRoleCommand(Guid.Empty);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsError.ShouldBe(true);
        result.FirstError.ShouldBeOfType(Errors.Role.RoleNotFound.GetType());
        _roleRepository.Verify(x => x.GetByIdAsync(Guid.Empty), Times.Once);
    }
}