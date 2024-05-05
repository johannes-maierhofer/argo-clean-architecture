namespace Argo.CA.Api.IntegrationTests.Testing;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        
        builder.ConfigureTestServices(_ =>
        {
            // add MassTransit TestHarness
            // see https://masstransit.io/documentation/concepts/testing
            //services.AddMassTransitTestHarness(_ => { });
        });
    }
}