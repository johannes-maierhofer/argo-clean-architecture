using Ardalis.GuardClauses;
using Argo.CA.Application.Common.Auth;
using Argo.CA.Application.Common.Persistence;
using Argo.CA.Domain.Common.Events;
using Argo.CA.Infrastructure.Identity;
using Argo.CA.Infrastructure.Logging;
using Argo.CA.Infrastructure.OpenTelemetry;
using Argo.CA.Infrastructure.Persistence;
using Argo.CA.Infrastructure.Persistence.Interceptors;
using Argo.CA.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Argo.CA.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        services
            .AddHttpContextAccessor()
            .AddPersistence(configuration)
            .AddAuth()
            .AddCustomSerilog()
            .AddCustomOpenTelemetry(configuration);

        services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();
        services.AddSingleton(TimeProvider.System);

        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("CaDemoDb");
        Guard.Against.Null(connectionString, message: "Connection string 'CaDemoDb' not found.");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DomainEventDispatchingInterceptor>();

        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            options.UseSqlServer(connectionString,
                providerOptions => providerOptions.EnableRetryOnFailure());

            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
        });

        services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());
        services.AddScoped(sp => sp.GetRequiredService<AppDbContext>().Database);
        services.AddScoped<AppDbContextInitializer>();

        return services;
    }

    private static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services.AddAuthentication()
            .AddBearerToken(IdentityConstants.BearerScheme);

        services.AddAuthorizationBuilder();

        services
            .AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddApiEndpoints();

        // add auth policies
        services.AddAuthorization(options =>
            {
                options.AddPolicy(Policies.CanCreateCompanies, policy => policy.RequireRole(Roles.Admin, Roles.Editor));
                options.AddPolicy(Policies.CanUpdateCompanies, policy => policy.RequireRole(Roles.Admin, Roles.Editor));
                options.AddPolicy(Policies.CanDeleteCompanies, policy => policy.RequireRole(Roles.Admin, Roles.Editor));
            }
        );

        return services;
    }
}