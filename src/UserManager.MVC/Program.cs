using System.Reflection;

using Mapster;

using MapsterMapper;

using Microsoft.AspNetCore.Authentication.Cookies;

using UserManager.MVC.Contracts;
using UserManager.MVC.Services;
using UserManager.MVC.Services.Base;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddHttpContextAccessor();
    builder.Services.Configure<CookiePolicyOptions>(
        options => { options.MinimumSameSitePolicy = SameSiteMode.None; });
    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie();

    builder.Services.AddHttpClient<IClient, Client>(options =>
        options.BaseAddress = new Uri("https://localhost:7039/"));

    var config = TypeAdapterConfig.GlobalSettings;
    config.Scan(Assembly.GetExecutingAssembly());
    builder.Services.AddSingleton(config);
    builder.Services.AddScoped<IMapper, ServiceMapper>();

    // Services
    builder.Services.AddSingleton<ILocalStorageService, LocalStorageService>();
    builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();

    builder.Services.AddScoped<IRoleService, RoleService>();
    builder.Services.AddScoped<IUserService, UserService>();

    builder.Services.AddControllersWithViews();
}

var app = builder.Build();
{
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    app.UseCookiePolicy();
    app.UseAuthentication();

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}