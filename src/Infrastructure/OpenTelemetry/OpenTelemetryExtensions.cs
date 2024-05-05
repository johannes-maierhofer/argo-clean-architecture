using Argo.CA.Application.Common.Telemetry;
using Argo.CA.Infrastructure.Configuration;
using Argo.CA.Infrastructure.Logging;
using Argo.CA.Infrastructure.Tracing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Diagnostics;

namespace Argo.CA.Infrastructure.OpenTelemetry;

public static class OpenTelemetryExtensions
{
    public static IServiceCollection AddCustomOpenTelemetry(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var appOptions = configuration.GetRequiredOptions<AppOptions>("App");
        var logOptions = configuration.GetRequiredOptions<LogOptions>("Log");
        var otlpOptions = configuration.GetRequiredOptions<OpenTelemetryOptions>("OpenTelemetry");

        // add custom ActivitySource for application
        Telemetry.ActivitySource = new ActivitySource(appOptions.ServiceName);

        // configure tracing
        services
            .AddOpenTelemetry()
            .WithTracing(builder =>
            {
                builder
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
                    .AddSource(appOptions.ServiceName); // app-specific source from Telemetry.ActivitySource

                if (!string.IsNullOrEmpty(otlpOptions.OtlpExporterUrl))
                {
                    builder.AddOtlpExporter(o =>
                    {
                        o.Endpoint = new Uri(otlpOptions.OtlpExporterUrl); // Jaeger endpoint
                    });
                }
            });

        return services;
    }
}