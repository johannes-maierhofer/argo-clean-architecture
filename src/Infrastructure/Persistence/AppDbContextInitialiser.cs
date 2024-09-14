using Argo.CA.Domain.UserAggregate;

namespace Argo.CA.Infrastructure.Persistence;

using Argo.CA.Application.Common.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public static class InitializerExtensions
{
    public static async Task InitialiseDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var initializer = scope.ServiceProvider.GetRequiredService<AppDbContextInitializer>();

        await initializer.InitialiseAsync();

        await initializer.SeedAsync();
    }
}

public class AppDbContextInitializer(
    ILogger<AppDbContextInitializer> logger,
    AppDbContext context,
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager)
{
    public async Task InitialiseAsync()
    {
        try
        {
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        // Default roles
        await EnsureRoleIsCreated(Roles.Admin);
        await EnsureRoleIsCreated(Roles.Editor);
        await EnsureRoleIsCreated(Roles.Reader);

        // Default users
        await EnsureUserIsCreated(
            "admin@argo-ca.com",
            "admin@argo-ca.com",
            "Admin1!",
            [Roles.Admin]);

        await EnsureUserIsCreated(
            "editor@argo-ca.com",
            "editor@argo-ca.com",
            "Editor1!",
            [Roles.Editor]);

        await EnsureUserIsCreated(
            "reader@argo-ca.com",
            "reader@argo-ca.com",
            "Reader1!",
            [Roles.Editor]);
    }

    private async Task EnsureRoleIsCreated(string roleName)
    {
        var role = new IdentityRole(roleName);

        if (roleManager.Roles.All(r => r.Name != role.Name))
        {
            await roleManager.CreateAsync(role);
        }
    }

    private async Task EnsureUserIsCreated(
        string userName,
        string email,
        string password,
        string[] roles)
    {
        var user = new User { UserName = userName, Email = email };

        if (userManager.Users.All(u => u.UserName != user.UserName))
        {
            await userManager.CreateAsync(user, password);

            if (roles.Any())
            {
                await userManager.AddToRolesAsync(user, roles);
            }
        }
    }
}
