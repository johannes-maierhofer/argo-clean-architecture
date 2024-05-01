namespace Argo.CA.Api.IntegrationTests;

using Microsoft.AspNetCore.Mvc.Testing;
using Testing;
using Xunit.Abstractions;

[Collection("IntegrationTests")]
public abstract class IntegrationTestBase(
    ITestOutputHelper output,
    CustomWebApplicationFactory factory,
    DatabaseFixture database)
    : IAsyncLifetime, IClassFixture<CustomWebApplicationFactory>
{
    protected ITestOutputHelper Output { get; } = output;
    protected WebApplicationFactory<Program> Factory { get; } = factory.WithTestLogging(output);

    public async Task InitializeAsync()
    {
        // reset the Db before every single test
        await database.ResetAsync();
    }

    public async Task DisposeAsync()
    {
        await Factory.DisposeAsync();
    }

    protected async Task AddEntityToDb<T>(T entity) where T : class
    {
        await using var dbContext = Factory.CreateDbContext();
        await dbContext.Set<T>().AddAsync(entity);
        await dbContext.SaveChangesAsync();
    }

    protected async Task AddEntityRangeToDb<T>(IEnumerable<T> entities) where T : class
    {
        await using var dbContext = Factory.CreateDbContext();
        await dbContext.Set<T>().AddRangeAsync(entities);
        await dbContext.SaveChangesAsync();
    }
}