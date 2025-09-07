using Argo.CA.Api.IntegrationTests.Testing.Authentication;

namespace Argo.CA.Api.IntegrationTests.Testing;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

public class CustomWebApplicationFactory(DatabaseFixture database) : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((_, config) =>
        {
            var testConfig = new Dictionary<string, string?>
            {
                { "ConnectionStrings:CaDemoDb", database.ConnectionString }
            };

            config.AddInMemoryCollection(testConfig);
        });

        builder.ConfigureTestServices(services =>
        {
            services.AddTestJwtBearerAuthentication();

            // add MassTransit TestHarness
            // see https://masstransit.io/documentation/concepts/testing
            //services.AddMassTransitTestHarness(_ => { });
        });
    }
}