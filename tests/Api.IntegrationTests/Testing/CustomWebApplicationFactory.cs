namespace Argo.CA.Api.IntegrationTests.Testing;

using Application.Common.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using TestDoubles;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        
        builder.ConfigureTestServices(services =>
        {
            // add MassTransit TestHarness
            // see https://masstransit.io/documentation/concepts/testing
            //services.AddMassTransitTestHarness(_ => { });

            // TODO: add test authentication, instead of using a dummy for current user
            services.AddScoped<ICurrentUserService, FakeCurrentUserService>();
        });
    }
}