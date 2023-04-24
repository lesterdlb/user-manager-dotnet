using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using UserManager.Api.Services;
using UserManager.Application;
using UserManager.Application.Common.Interfaces.Services;
using UserManager.Infrastructure;
using UserManager.Infrastructure.Persistence;

namespace UserManager.Api;

public static class DependencyInjection
{
    private const string CorsPolicy = "CorsPolicy";

    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        AddSwagger(builder.Services);

        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);

        builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();
        builder.Services.AddProblemDetails();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        AddCors(builder.Services);

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(
                    "/swagger/v1/swagger.json",
                    "User Management API");
            });
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();

        app.UseExceptionHandler();

        app.UseCors(CorsPolicy);
        app.UseAuthorization();
        app.MapControllers();
        app.UseStaticFiles();

        return app;
    }

    public static async Task ResetDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        try
        {
            var context = scope.ServiceProvider.GetService<UserManagerIdentityDbContext>();
            var initializer = scope.ServiceProvider.GetRequiredService<UserManagerIdentityDbContextInitializer>();

            if (context is not null && !context.Database.IsInMemory())
            {
                await context.Database.EnsureDeletedAsync();
                await initializer.InitialiseAsync();
                await initializer.SeedAsync();
            }
        }
        catch (Exception ex)
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
            logger.LogError(ex, "An error occurred while migrating the database.");
        }
    }

    private static void AddSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description =
                    "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });

            options.SwaggerDoc(
                "v1",
                new OpenApiInfo { Title = "UserManager", Version = "v1" });
        });
    }

    private static void AddCors(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(
                CorsPolicy,
                policyBuilder => policyBuilder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });
    }
}