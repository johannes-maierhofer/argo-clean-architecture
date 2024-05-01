namespace Argo.CA.Infrastructure.Tracing;

using System.Diagnostics;
using Application.Common.Telemetry;
using Configuration;
using Logging;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

public static class OpenTelemetryExtensions
{
    public static IServiceCollection AddCustomOpenTelemetry(this IServiceCollection services)
    {
        var configuration = services.GetConfiguration();
        var appOptions = configuration.GetRequiredOptions<AppOptions>("App");
        var logOptions = configuration.GetRequiredOptions<LogOptions>("Log");

        // add custom ActivitySource for application
        Telemetry.ActivitySource = new ActivitySource(appOptions.ServiceName);

        // configure tracing
        services
            .AddOpenTelemetry()
            .WithTracing(builder => builder
                .AddAspNetCoreInstrumentation(options =>
                {
                    options.Filter = context =>
                    {
                        if (context.Request.Path.ToString().EndsWith("/healthz"))
                        {
                            return false;
                        }

                        return true;
                    };
                })
                .AddHttpClientInstrumentation(options =>
                {
                    options.FilterHttpRequestMessage = message =>
                    {
                        // ignore http calls to seq
                        var requestUrl = message.RequestUri?.ToString() ?? string.Empty;
                        if (requestUrl.StartsWith(logOptions.Seq.ServiceUrl))
                        {
                            return false;
                        }

                        // ignore http calls to health-check
                        if (requestUrl.ToLower().EndsWith("/healthz"))
                        {
                            return false;
                        }

                        return true;
                    };
                })
                .AddEntityFrameworkCoreInstrumentation(opt =>
                {
                    opt.SetDbStatementForText = true;
                    opt.SetDbStatementForStoredProcedure = true;
                    opt.EnrichWithIDbCommand = (activity, command) =>
                    {
                        var stateDisplayName = $"Execute command type {command.CommandType}";
                        activity.DisplayName = stateDisplayName;
                        activity.SetTag("db.commandType", command.CommandType);
                    };
                })
                .SetResourceBuilder(ResourceBuilder
                    .CreateDefault()
                    .AddService(appOptions.ServiceName))
                .AddSource("MassTransit")
                .AddSource(appOptions.ServiceName) // app-specific source from Telemetry.ActivitySource
                .AddOtlpExporter(o =>
                {
                    // TODO: get Jaeger endpoint from configuration
                    o.Endpoint = new Uri("http://localhost:4317"); // Jaeger endpoint
                })
            );

        // TODO: Use the OpenTelemetry Collector instead of directly exporting to Jaeger

        return services;
    }
}