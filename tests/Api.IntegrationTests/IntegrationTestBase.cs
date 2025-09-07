using Argo.CA.Infrastructure.Persistence;

namespace Argo.CA.Api.IntegrationTests;

using Microsoft.AspNetCore.Mvc.Testing;
using Testing;
using Xunit.Abstractions;

[Collection("IntegrationTests")]
public abstract class IntegrationTestBase(
    ITestOutputHelper output,
    DatabaseFixture database)
    : IAsyncLifetime, IClassFixture<CustomWebApplicationFactory>
{
    protected ITestOutputHelper Output { get; } = output;

    public async Task InitializeAsync()
    {
        // reset the Db before every single test
        await database.ResetAsync();
    }

    public async Task DisposeAsync()
    {
        await database.DisposeAsync();
    }

    protected WebApplicationFactory<Program> CreateWebAppFactory()
    {
        return new CustomWebApplicationFactory(database)
            .WithTestLogging(Output);
    }

    protected AppDbContext CreateDbContext() => database.CreateDbContext();

    protected async Task AddEntityToDb<T>(T entity) where T : class
    {
        await using var dbContext = database.CreateDbContext();
        await dbContext.Set<T>().AddAsync(entity);
        await dbContext.SaveChangesAsync();
    }

    protected async Task AddEntityRangeToDb<T>(IEnumerable<T> entities) where T : class
    {
        await using var dbContext = database.CreateDbContext();
        await dbContext.Set<T>().AddRangeAsync(entities);
        await dbContext.SaveChangesAsync();
    }
}