using System.Data.Common;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Respawn;

using UserManager.Application.IntegrationTests.Authentication;
using UserManager.Application.IntegrationTests.Extensions;
using UserManager.Infrastructure.Persistence;

namespace UserManager.Application.IntegrationTests.Base;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>, IAsyncLifetime
    where TStartup : class
{
    public HttpClient HttpClient { get; private set; } = default!;

    private const string DefaultUserId = "00000000-0000-0000-0000-000000000001";

    private const string ConnectionString =
        "Server=(localdb)\\mssqllocaldb;Database=UserManagerDbTest;Trusted_Connection=True;MultipleActiveResultSets=true";

    private Respawner _respawner = default!;
    private DbConnection _dbConnection = default!;

    public async Task InitializeAsync()
    {
        await InitializeDatabase();

        _dbConnection = new SqlConnection(ConnectionString);
        await _dbConnection.OpenAsync();
        HttpClient = CreateClient();
        _respawner = await Respawner.CreateAsync(
            _dbConnection,
            new RespawnerOptions { DbAdapter = DbAdapter.SqlServer, WithReseed = true });
    }

    public new async Task DisposeAsync() => await _dbConnection.CloseAsync();

    public async Task ResetDatabase()
    {
        await _respawner.ResetAsync(_dbConnection);
    }

    public async Task ReSeedDatabase()
    {
        using var scope = Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<UserManagerIdentityDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

        try
        {
            await Utilities.SeedDatabase(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"An error occurred seeding the database with test messages. Error: {ex.Message}");
        }
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services
                .Remove<DbContextOptions<UserManagerIdentityDbContext>>()
                .AddDbContext<UserManagerIdentityDbContext>(options =>
                {
                    options.UseSqlServer(ConnectionString);
                });

            services.Configure<TestAuthHandlerOptions>(options => options.DefaultUserId = DefaultUserId);
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = TestAuthHandler.AuthenticationScheme;
                    options.DefaultScheme = TestAuthHandler.AuthenticationScheme;
                    options.DefaultChallengeScheme = TestAuthHandler.AuthenticationScheme;
                })
                .AddScheme<TestAuthHandlerOptions, TestAuthHandler>(
                    TestAuthHandler.AuthenticationScheme,
                    options => { });
        });
    }

    private async Task InitializeDatabase()
    {
        using var scope = Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<UserManagerIdentityDbContext>();

        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }
}