namespace Argo.CA.Api.IntegrationTests.Testing;

using ApiClients;
using CA.Infrastructure.Logging;
using CA.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Xunit.Abstractions;

public static class WebApplicationFactoryExtensions
{
    public static WebApplicationFactory<Program> WithTestLogging(
        this CustomWebApplicationFactory factory,
        ITestOutputHelper output)
    {
        return factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddCustomSerilog(cfg =>
                {
                    // customize log levels
                    cfg.MinimumLevel.Is(LogEventLevel.Debug)
                        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
                        .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
                        .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Information);

                    // make Serilog write to test output via Serilog.Sinks.XUnit
                    cfg.WriteTo.TestOutput(output);
                });
            });
        });
    }

    public static ApiClient CreateApiClient(this WebApplicationFactory<Program> factory)
    {
        var httpClient = factory.CreateClient();
        return new ApiClient(httpClient.BaseAddress?.ToString(), httpClient);
    }

    /// <summary>
    /// Creates a DbContext without any of the extra configuration like interceptors etc.
    /// </summary>
    /// <param name="factory"></param>
    /// <returns></returns>
    public static AppDbContext CreateDbContext(this WebApplicationFactory<Program> factory)
    {
        var configuration = factory.Services.GetRequiredService<IConfiguration>();
        var connectionString = configuration.GetConnectionString("CaDemoDb");
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new AppDbContext(options);
    }
}