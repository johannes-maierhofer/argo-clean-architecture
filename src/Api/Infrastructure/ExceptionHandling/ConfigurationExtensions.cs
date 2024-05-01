namespace Argo.CA.Api.Infrastructure.ExceptionHandling;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddCustomExceptionHandlers(this IServiceCollection services)
    {
        // If using ExceptionHandlers, also make sure to filter out logging by Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware
        // see https://github.com/dotnet/aspnetcore/issues/19740

        // the order is important, do not change!!!
        services.AddExceptionHandler<LoggingExceptionHandler>();
        services.AddExceptionHandler<CustomExceptionHandler>();

        return services;
    }

    public static IApplicationBuilder UseCustomExceptionHandlers(this IApplicationBuilder app)
    {
        return app.UseExceptionHandler(_ => { });
    }
}