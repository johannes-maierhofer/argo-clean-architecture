namespace Argo.CA.Infrastructure.Logging;

using Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Events;
using Serilog.Exceptions;

public static class SerilogExtensions
{
    public static IServiceCollection AddCustomSerilog(
        this IServiceCollection services, 
        Action<LoggerConfiguration>? configure = null)
    {
        var configuration = services.GetConfiguration();
        var appOptions = configuration.GetRequiredOptions<AppOptions>("App");
        var logOptions = configuration.GetRequiredOptions<LogOptions>("Log");

        var logLevel = Enum.TryParse<LogEventLevel>(logOptions.Level, true, out var level)
            ? level
            : LogEventLevel.Information;

        services.AddSerilog(loggerConfiguration =>
        {
            loggerConfiguration
                .MinimumLevel.Is(logLevel)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("MassTransit", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Information)
                // see https://github.com/dotnet/aspnetcore/issues/46280
                .MinimumLevel.Override("Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware", LogEventLevel.Fatal)
                // Filter out ASP.NET Core infrastructure logs that are Information and below
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .MinimumLevel.Override("System.Net.Http.HttpClient.health-checks", LogEventLevel.Error)
                //.MinimumLevel.Override("Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware", LogEventLevel.Fatal)
                .Enrich.WithExceptionDetails()
                .Enrich.WithSpan() // span information (=trace) from current activity
                .Enrich.FromLogContext()
                .Enrich.WithEnvironmentName()
                .Enrich.WithProperty("ServiceName", appOptions.ServiceName)
                .ReadFrom.Configuration(configuration);

            if (logOptions.Seq.Enabled)
            {
                loggerConfiguration.WriteTo.Seq(logOptions.Seq.ServiceUrl);
            }

            configure?.Invoke(loggerConfiguration);
        });

        return services;
    }
}