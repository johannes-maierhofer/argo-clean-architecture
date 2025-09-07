using Argo.CA.Infrastructure.Persistence;

namespace Argo.CA.Api.IntegrationTests.Testing;

using Database;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly ITestDatabase _database = new TestContainersTestDatabase();

    public string ConnectionString => _database.ConnectionString;

    public async Task InitializeAsync()
    {
        await _database.InitialiseAsync();
    }

    public async Task ResetAsync()
    {
        await _database.ResetAsync();
    }

    public async Task DisposeAsync()
    {
        await _database.DisposeAsync();
    }

    public AppDbContext CreateDbContext()
    {
        return _database.CreateDbContext();
    }
}
