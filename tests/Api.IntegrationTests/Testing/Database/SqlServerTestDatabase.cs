namespace Argo.CA.Api.IntegrationTests.Testing.Database;

using Ardalis.GuardClauses;
using CA.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Respawn;

public class SqlServerTestDatabase : ITestDatabase
{
    private Respawner _respawner = null!;
    private readonly string _connectionString;

    public SqlServerTestDatabase()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Testing.json", false)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("CaDemoDb");
        Guard.Against.Null(connectionString);
        _connectionString = connectionString;
    }

    public async Task InitialiseAsync()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(_connectionString)
            .Options;

        var context = new AppDbContext(options);

        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();

        _respawner = await Respawner.CreateAsync(_connectionString, new RespawnerOptions
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
        await _respawner.ResetAsync(_connectionString);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}
