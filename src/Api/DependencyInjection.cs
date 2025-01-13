using Argo.CA.Api.Infrastructure.ExceptionHandling;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace Argo.CA.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Program));

        services
            .AddExceptionHandlers()
            .AddCustomSwaggerGen();

        services.AddControllers();
        
        // Customize default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddEndpointsApiExplorer();

        return services;
    }

    private static IServiceCollection AddExceptionHandlers(this IServiceCollection services)
    {
        // the order is important, do not change!!!
        services.AddExceptionHandler<LoggingExceptionHandler>();
        services.AddExceptionHandler<CustomExceptionHandler>();

        return services;
    }

    private static IServiceCollection AddCustomSwaggerGen(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "CA API", Version = "v1" });
            c.EnableAnnotations();
            c.SupportNonNullableReferenceTypes();

            c.AddSecurityDefinition("Bearer",
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    new string[] { }
                }
            });
        });

        return services;
    }
}
