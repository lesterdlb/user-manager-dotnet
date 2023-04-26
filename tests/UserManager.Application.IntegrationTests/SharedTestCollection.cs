using UserManager.Application.IntegrationTests.Base;

namespace UserManager.Application.IntegrationTests;

[CollectionDefinition("IntegrationTests")]
public class SharedTestCollection : IClassFixture<CustomWebApplicationFactory<Program>>
{
}