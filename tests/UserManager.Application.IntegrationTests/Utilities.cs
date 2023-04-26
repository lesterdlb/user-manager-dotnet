﻿using Microsoft.AspNetCore.Identity;

using UserManager.Infrastructure.Identity;
using UserManager.Infrastructure.Persistence;

namespace UserManager.Application.IntegrationTests;

public class Utilities
{
    private const string AdminRoleId = "00000000-0000-0000-0000-000000000001";
    private const string UserRoleId = "00000000-0000-0000-0000-000000000002";
    private const string AdminUserId = "00000000-0000-0000-0000-000000000003";
    private const string UserUserId = "00000000-0000-0000-0000-000000000004";

    public static string GetUserRoleId => UserRoleId;

    public static async Task SeedDatabase(UserManagerIdentityDbContext context)
    {
        var roles = new List<IdentityRole>
        {
            new() { Id = AdminRoleId, Name = "Admin", NormalizedName = "ADMIN" },
            new() { Id = UserRoleId, Name = "User", NormalizedName = "USER" }
        };

        var users = new List<ApplicationUser>
        {
            new() { Id = AdminUserId, UserName = "admin", NormalizedUserName = "ADMIN" },
            new() { Id = UserUserId, UserName = "user", NormalizedUserName = "USER" }
        };

        context.Roles.AddRange(roles);
        context.Users.AddRange(users);
        await context.SaveChangesAsync();
    }
}