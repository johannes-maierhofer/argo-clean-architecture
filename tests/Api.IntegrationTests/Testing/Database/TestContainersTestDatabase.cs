namespace Argo.CA.Api.IntegrationTests.Testing.Database;

using Ardalis.GuardClauses;
using CA.Infrastructure.Persistence;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Respawn;

// Make sure you have Docker running
public class TestContainersTestDatabase : ITestDatabase
{
    private const string Username = "sa";
    private const string Password = "str0ngp@ssword";
    private const ushort MsSqlPort = 1433;
    private const ushort HostPort = 58825;

    private readonly IContainer _mssqlContainer = new ContainerBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2019-latest")
        .WithName("mssql-" + Guid.NewGuid())
        .WithPortBinding(HostPort, MsSqlPort)
        .WithEnvironment("ACCEPT_EULA", "Y")
        .WithEnvironment("SQLCMDUSER", Username)
        .WithEnvironment("SQLCMDPASSWORD", Password)
        .WithEnvironment("MSSQL_SA_PASSWORD", Password)
        .WithWaitStrategy(Wait.ForUnixContainer().UntilCommandIsCompleted("/opt/mssql-tools18/bin/sqlcmd", "-C", "-Q", "SELECT 1;"))
        .Build();

    private Respawner _respawner = null!;
    private readonly string _connectionString;

    public TestContainersTestDatabase()
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
        // start the test-container
        await _mssqlContainer.StartAsync();

        // create the Db
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(_connectionString)
            .Options;

        var context = new AppDbContext(options);

        await context.Database.EnsureDeletedAsync();
        await context.Database.MigrateAsync();

        _respawner = await Respawner.CreateAsync(_connectionString, new RespawnerOptions
        {
            TablesToIgnore = ["__EFMigrationsHistory"]
        });
    }

    public async Task ResetAsync()
    {
        await _respawner.ResetAsync(_connectionString);
    }

    public async Task DisposeAsync()
    {
        await _mssqlContainer.DisposeAsync();
    }
}
