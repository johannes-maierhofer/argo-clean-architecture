namespace Argo.CA.Infrastructure;

using Application.Common.Persistence;
using Ardalis.GuardClauses;
using Domain.Common.Events;
using Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;
using Persistence.Interceptors;
using Services;
using Tracing;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        services.AddPersistence(configuration);

        if (!environment.IsEnvironment("Testing"))
        {
            services
                .AddCustomSerilog()
                .AddCustomOpenTelemetry();
        }

        services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();
        services.AddSingleton(TimeProvider.System);

        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("CaDemoDb");
        Guard.Against.Null(connectionString, message: "Connection string 'CaDemoDb' not found.");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DomainEventDispatchingInterceptor>();

        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            options.UseSqlServer(connectionString,
                providerOptions => providerOptions.EnableRetryOnFailure());
            
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
        });

        // make sure IAppDbContext and ITransactionalDbContext return the same instance of the DbContext
        services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());
        services.AddScoped<ITransactionalDbContext>(sp => sp.GetRequiredService<AppDbContext>());
        services.AddScoped<AppDbContextInitializer>();

        return services;
    }
}