namespace Argo.CA.Infrastructure.Messaging;

using System.Reflection;
using Configuration;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class MassTransitExtensions
{
    /// <summary>
    /// Adds MassTransit and sets up the outbox using the given DbContext.
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static IServiceCollection AddCustomMassTransit<TDbContext>(
        this IServiceCollection services,
        params Assembly[] assemblies)
        where TDbContext : DbContext
    {
        return services
            .AddCustomMassTransit<TDbContext>(
                null,
                null,
                assemblies);
    }

    /// <summary>
    /// Adds MassTransit and sets up the outbox using the given DbContext.
    /// </summary>
    /// <typeparam name="TDbContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="customConfigure"></param>
    /// <param name="customRmqConfigure"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    public static IServiceCollection AddCustomMassTransit<TDbContext>(
        this IServiceCollection services,
        Action<IBusRegistrationConfigurator>? customConfigure,
        Action<IRabbitMqBusFactoryConfigurator>? customRmqConfigure,
        params Assembly[] assemblies)
        where TDbContext : DbContext
    {
        AddMasstransit(
            services,
            configure =>
            {
                configure.AddEntityFrameworkOutbox<TDbContext>(o =>
                {
                    o.QueryDelay = TimeSpan.FromSeconds(5);
                    o.UseSqlServer();
                    o.UseBusOutbox();
                });

                customConfigure?.Invoke(configure);
            },
            customRmqConfigure,
            assemblies);

        return services;
    }

    public static IServiceCollection AddCustomMassTransit(
        this IServiceCollection services,
        params Assembly[] assemblies)
    {
        AddMasstransit(
            services,
            null,
            null,
            assemblies);

        return services;
    }

    public static IServiceCollection AddCustomMassTransit(
        this IServiceCollection services,
        Action<IBusRegistrationConfigurator>? customConfigure,
        Action<IRabbitMqBusFactoryConfigurator>? customRmqConfigure,
        params Assembly[] assemblies)
    {
        AddMasstransit(
            services,
            customConfigure,
            customRmqConfigure,
            assemblies);

        return services;
    }

    private static void AddMasstransit(
        IServiceCollection services,
        Action<IBusRegistrationConfigurator>? customConfigure,
        Action<IRabbitMqBusFactoryConfigurator>? customRmqConfigure,
        params Assembly[] assemblies)
    {
        var configuration = services.GetConfiguration();
        var appOptions = configuration.GetRequiredOptions<AppOptions>("App");
        var rabbitMqOptions = configuration.GetRequiredOptions<RabbitMqOptions>("RabbitMq");

        services.AddMassTransit(configure =>
        {
            configure.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("MT-" + appOptions.ServiceName + "--", false));

            if (assemblies.Length > 0)
                configure.AddConsumers(assemblies);

            configure.UsingRabbitMq((ctx, rmq) =>
            {
                rmq.Host(rabbitMqOptions.HostName, rabbitMqOptions.Port, rabbitMqOptions.VirtualHost, h =>
                {
                    h.Username(rabbitMqOptions.UserName);
                    h.Password(rabbitMqOptions.Password);
                });

                // MT per default uses System.Text.Json
                // however, System.Text.Json does not support serializing generic message types
                rmq.UseNewtonsoftJsonSerializer();

                // for Retries/Redeliveries
                // see https://masstransit.io/documentation/concepts/exceptions#retry-configuration
                rmq.UseMessageRetry(r => r.Exponential(
                    3,
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(1)));
                    
                rmq.UseDelayedRedelivery(r => r.Intervals(
                    TimeSpan.FromMinutes(5),
                    TimeSpan.FromMinutes(15),
                    TimeSpan.FromMinutes(30),
                    TimeSpan.FromHours(1),
                    TimeSpan.FromHours(4),
                    TimeSpan.FromHours(8),
                    TimeSpan.FromDays(1),
                    TimeSpan.FromDays(2))); // max 2 days

                // default concurrency limit
                rmq.ConcurrentMessageLimit = 4;

                rmq.ConfigureEndpoints(ctx);

                // overwrite with custom RMQ configuration
                customRmqConfigure?.Invoke(rmq);
            });

            // overwrite with custom configuration
            customConfigure?.Invoke(configure);
        });
    }
}