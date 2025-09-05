namespace Argo.CA.Api.IntegrationTests.Testing.Database;

using Ardalis.GuardClauses;
using CA.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Respawn;

public class SqlServerTestDatabase : ITestDatabase
{
    private Respawner _respawner = null!;

    public SqlServerTestDatabase()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Testing.json", false)
            .AddEnvironmentVariables()
            .Build();

        var connString = configuration.GetConnectionString("CaDemoDb");
        Guard.Against.Null(connString);
        this.ConnectionString = connString;
    }

    public string ConnectionString { get; }

    public async Task InitialiseAsync()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(ConnectionString)
            .Options;

        var context = new AppDbContext(options);

        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();

        _respawner = await Respawner.CreateAsync(ConnectionString, new RespawnerOptions
        {
            TablesToIgnore = [
                "__EFMigrationsHistory",
                "AspNetRoleClaims",
                "AspNetRoles",
                "AspNetUserClaims",
                "AspNetUserLogins",
                "AspNetUserRoles",
                "AspNetUsers",
                "AspNetUserTokens"
            ]
        });
    }

    public async Task ResetAsync()
    {
        await _respawner.ResetAsync(ConnectionString);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    public AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(ConnectionString)
            .EnableSensitiveDataLogging()
            .Options;

        return new AppDbContext(options);
    }
}
