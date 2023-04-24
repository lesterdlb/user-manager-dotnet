using Moq;

using Shouldly;

using UserManager.Application.Common.DTOs.Role;
using UserManager.Application.Common.Interfaces.Repositories;
using UserManager.Application.Features.Roles.Queries.GetRoles;

namespace UserManager.Application.UnitTests.Roles.Queries;

public class GetRolesQueryHandlerTests
{
    private readonly Mock<IRoleRepository> _roleRepository;

    public GetRolesQueryHandlerTests()
    {
        _roleRepository = Mocks.RepositoryMocks.GetRoleRepository();
    }

    [Fact]
    public async Task GetRolesTest()
    {
        var handler = new GetRolesQueryHandler(_roleRepository.Object);

        var result = await handler.Handle(new GetRolesQuery(), CancellationToken.None);

        result.ShouldBeOfType<List<RoleDto>>();

        result.Count.ShouldBe(3);
    }
}