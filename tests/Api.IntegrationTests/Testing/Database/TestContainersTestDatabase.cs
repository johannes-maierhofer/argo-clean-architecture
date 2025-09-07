using Argo.CA.Infrastructure.Persistence;

namespace Argo.CA.Api.IntegrationTests.Testing.Database;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Respawn;
using Testcontainers.MsSql;

// Make sure you have Docker running
public class TestContainersTestDatabase : ITestDatabase
{
    private readonly MsSqlContainer _mssqlContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .WithReuse(true)
        .WithLabel("reuse-id", "CaDemoDb")
        .Build();

    private Respawner _respawner = null!;

    public string ConnectionString { get; private set; } = string.Empty;

    public async Task InitialiseAsync()
    {
        // start the test-container
        await _mssqlContainer.StartAsync();

        var builder = new SqlConnectionStringBuilder(_mssqlContainer.GetConnectionString())
        {
            InitialCatalog = "CaDemoDb"
        };

        ConnectionString = builder.ConnectionString;

        // create the Db
        await using var dbContext = CreateDbContext();
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.MigrateAsync();

        // init respawner
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
        // nothing to dispose
        // Keeping the test container running enhances the developer experience
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
