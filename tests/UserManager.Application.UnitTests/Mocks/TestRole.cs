using UserManager.Domain.Entities;

namespace UserManager.Application.UnitTests.Mocks;

public class TestRole : Role
{
    public TestRole(Guid id) => Id = id;
}