using Ardalis.GuardClauses;
using Argo.CA.Application.Common.Persistence;
using Argo.CA.Application.Common.Security;
using Argo.CA.Application.Common.Security.CurrentUserProvider;
using Argo.CA.Application.Common.Security.JwtTokenGeneration;
using Argo.CA.Application.Common.Security.Policies;
using Argo.CA.Domain.Common.Events;
using Argo.CA.Domain.UserAggregate;
using Argo.CA.Infrastructure.Logging;
using Argo.CA.Infrastructure.OpenTelemetry;
using Argo.CA.Infrastructure.Persistence;
using Argo.CA.Infrastructure.Persistence.Interceptors;
using Argo.CA.Infrastructure.Security;
using Argo.CA.Infrastructure.Security.JwtTokenGeneration;
using Argo.CA.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Argo.CA.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddHttpContextAccessor()
            .AddPersistence(configuration)
            .AddAuthentication(configuration)
            .AddAuthorization()
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

    private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.Section));

        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

        services
            .ConfigureOptions<JwtBearerTokenValidationConfiguration>()
            .AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        services
            .AddIdentityCore<User>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>();

        return services;
    }

    private static IServiceCollection AddAuthorization(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder();

        services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();

        // add auth policies
        services.AddAuthorization(options =>
            {
                options.AddPolicy(Policy.Company.Get, policy => policy.RequireRole(Roles.Admin, Roles.Editor, Roles.Reader));
                options.AddPolicy(Policy.Company.Create, policy => policy.RequireRole(Roles.Admin, Roles.Editor));
                options.AddPolicy(Policy.Company.Update, policy => policy.RequireRole(Roles.Admin, Roles.Editor));
                options.AddPolicy(Policy.Company.Delete, policy => policy.RequireRole(Roles.Admin, Roles.Editor));
            }
        );

        return services;
    }
}