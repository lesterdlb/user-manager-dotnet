using UserManager.Api;
using UserManager.Application;
using UserManager.Infrastructure;
using UserManager.Infrastructure.Persistence;

using DependencyInjection = UserManager.Api.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddApi();
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "UserManager v1"));

        using var scope = app.Services.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<UserManagerIdentityDbContextInitializer>();
        await initializer.InitialiseAsync();
        await initializer.SeedAsync();
    }

    app.UseAuthentication();
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.UseCors(DependencyInjection.CorsPolicy);
    app.MapControllers();
    app.UseStaticFiles();

    app.Run();
}