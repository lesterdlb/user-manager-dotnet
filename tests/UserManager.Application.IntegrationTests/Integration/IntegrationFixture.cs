using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using UserManager.Infrastructure.Identity;
using UserManager.Infrastructure.Persistence;

namespace UserManager.Application.IntegrationTests.Integration;

public class IntegrationFixture : IDisposable
{
    private static WebApplicationFactory<Program> _factory = null!;
    private static IServiceScopeFactory _scopeFactory = null!;

    public IntegrationFixture()
    {
        _factory = new CustomWebApplicationFactory<Program>();
        _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
    }

    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        return await mediator.Send(request);
    }

    public static async Task SeedDatabase()
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await context.Database.EnsureDeletedAsync();

        context.Users.Add(new ApplicationUser
        {
            UserName = "testUser",
            Email = "testUser@localhost",
            EmailConfirmed = true,
            FirstName = "Test",
            LastName = "User",
            PhoneNumber = "1234567890",
            PhoneNumberConfirmed = true
        });
        await context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _factory.Dispose();
    }
}