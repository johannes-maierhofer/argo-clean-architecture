using Argo.CA.Infrastructure.Persistence;

namespace Argo.CA.Api.IntegrationTests.Testing.Database;

public interface ITestDatabase
{
    string ConnectionString { get; }
    Task InitialiseAsync();
    Task ResetAsync();
    Task DisposeAsync();
    AppDbContext CreateDbContext();
}