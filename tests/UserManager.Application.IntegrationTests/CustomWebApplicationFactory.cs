using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using UserManager.Application.Common.Interfaces.Services;
using UserManager.Application.IntegrationTests.Extensions;
using UserManager.Infrastructure.Persistence;

namespace UserManager.Application.IntegrationTests;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
    where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices((_, services) =>
        {
            services
                .Remove<ICurrentUserService>()
                .AddTransient(_ => Mock.Of<ICurrentUserService>(u =>
                    u.UserId == Guid.NewGuid().ToString()));

            services
                .Remove<DbContextOptions<ApplicationDbContext>>()
                .AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
        });
    }
}