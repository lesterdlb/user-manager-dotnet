using UserManager.Application.IntegrationTests.Integration;

namespace UserManager.Application.IntegrationTests;

using static IntegrationFixture;

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