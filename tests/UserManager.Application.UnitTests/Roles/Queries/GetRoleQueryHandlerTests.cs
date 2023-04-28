using ErrorOr;

using Mapster;

using MapsterMapper;

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
    private readonly IMapper _mapper;

    public GetRoleQueryHandlerTests()
    {
        _roleRepository = RepositoryMocks.GetRoleRepository();
        var config = new TypeAdapterConfig();
        _mapper = new Mapper(config);
    }

    [Fact]
    public async Task GetRoleTest()
    {
        var handler = new GetRoleQueryHandler(_roleRepository.Object, _mapper);
        var query = new GetRoleQuery(RepositoryMocks.GetUserRoleId());

        var result = await handler.Handle(query, CancellationToken.None);

        result.ShouldBeOfType<ErrorOr<RoleDto>>();
        result.IsError.ShouldBeFalse();
        result.Value.Id.ShouldBe(RepositoryMocks.GetUserRoleId());
    }

    [Fact]
    public async Task GetRoleTest_InvalidId()
    {
        var handler = new GetRoleQueryHandler(_roleRepository.Object, _mapper);
        var query = new GetRoleQuery("00000000-0000-0000-0000-000000000000");

        var result = await handler.Handle(query, CancellationToken.None);

        result.ShouldBeOfType<ErrorOr<RoleDto>>();
        result.IsError.ShouldBeTrue();
        result.FirstError.ShouldBe(Errors.Role.RoleNotFound);
    }
}