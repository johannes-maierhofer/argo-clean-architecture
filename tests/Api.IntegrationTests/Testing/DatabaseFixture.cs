namespace Argo.CA.Api.IntegrationTests.Testing;

using Database;

public class DatabaseFixture : IAsyncLifetime
{
    private readonly ITestDatabase _database = new SqlServerTestDatabase();

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
}