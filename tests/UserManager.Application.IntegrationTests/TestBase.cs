using static UserManager.Application.IntegrationTests.Integration.IntegrationFixture;

namespace UserManager.Application.IntegrationTests;
public class TestBase : IAsyncLifetime
{
    public async Task InitializeAsync()
    {
        await SeedDatabase();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}