using System.Reflection;
using System.Text;

using Mapster;

using MapsterMapper;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using UserManager.Application.Common.Interfaces;
using UserManager.Application.Common.Interfaces.Authentication;
using UserManager.Application.Common.Interfaces.Repositories;
using UserManager.Application.Common.Interfaces.Services;
using UserManager.Infrastructure.Authentication;
using UserManager.Infrastructure.Identity;
using UserManager.Infrastructure.Persistence;
using UserManager.Infrastructure.Persistence.Interceptors;
using UserManager.Infrastructure.Repositories;
using UserManager.Infrastructure.Services;

namespace UserManager.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

        AddMapping(services);
        AddDbContext(services, configuration);

        services.AddTransient<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IIdentityService, IdentityService>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();

        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"] ?? string.Empty))
                };
            });
    }

    private static void AddMapping(IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        services.AddDbContext<UserManagerIdentityDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                builder => builder.MigrationsAssembly(typeof(UserManagerIdentityDbContext).Assembly.FullName)));

        services.AddScoped<UserManagerIdentityDbContextInitializer>();
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<UserManagerIdentityDbContext>()
            .AddDefaultTokenProviders();
    }
}