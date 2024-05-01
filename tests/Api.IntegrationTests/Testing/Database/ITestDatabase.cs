namespace Argo.CA.Api.IntegrationTests.Testing.Database;

public interface ITestDatabase
{
    Task InitialiseAsync();
    Task ResetAsync();
    Task DisposeAsync();
}