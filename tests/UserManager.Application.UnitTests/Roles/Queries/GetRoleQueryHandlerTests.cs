using ErrorOr;

using Moq;

using Shouldly;

using UserManager.Application.Common.DTOs.Role;
using UserManager.Application.Common.Interfaces.Repositories;
using UserManager.Application.Features.Roles.Queries.GetRole;
using UserManager.Application.UnitTests.Mocks;
using UserManager.Domain.Common.Errors;

namespace UserManager.Application.UnitTests.Roles.Queries;

public class GetRoleQueryHandlerTests
{
    private readonly Mock<IRoleRepository> _roleRepository;

    public GetRoleQueryHandlerTests()
    {
        _roleRepository = Mocks.RepositoryMocks.GetRoleRepository();
    }

    [Fact]
    public async Task GetRoleTest()
    {
        var handler = new GetRoleQueryHandler(_roleRepository.Object);
        var query = new GetRoleQuery(RepositoryMocks.GetUserRoleId());

        var result = await handler.Handle(query, CancellationToken.None);

        result.ShouldBeOfType<ErrorOr<RoleDto>>();
        result.IsError.ShouldBeFalse();
        result.Value.Id.ShouldBe(RepositoryMocks.GetUserRoleId());
    }

    [Fact]
    public async Task GetRoleTest_InvalidId()
    {
        var handler = new GetRoleQueryHandler(_roleRepository.Object);
        var query = new GetRoleQuery("00000000-0000-0000-0000-000000000000");

        var result = await handler.Handle(query, CancellationToken.None);

        result.ShouldBeOfType<ErrorOr<RoleDto>>();
        result.IsError.ShouldBeTrue();
        result.FirstError.ShouldBe(Errors.Role.RoleNotFound);
    }
}