namespace Argo.CA.Api.IntegrationTests.Testing;

using ApiClients;
using Authentication;
using CA.Infrastructure.Logging;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
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
                        .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Information)
                        .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Query", LogEventLevel.Information);

                    // make Serilog write to test output via Serilog.Sinks.XUnit
                    cfg.WriteTo.TestOutput(output);
                });
            });
        });
    }

    public static ApiClient CreateApiClient(this WebApplicationFactory<Program> factory, Action<JwtTokenBuilder>? jwtBuilder = null)
    {
        jwtBuilder ??= builder =>
        {
            // per default the user has all the necessary claims
            builder.AsAdmin();
        };

        var httpClient = factory
            .CreateClient()
            .WithJwtBearerToken(jwtBuilder);

        return new ApiClient(httpClient.BaseAddress?.ToString(), httpClient);
    }
}