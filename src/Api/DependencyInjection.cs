namespace Argo.CA.Api;

using System.Reflection;
using Application.Common.Authorization;
using Infrastructure.ExceptionHandling;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using DotSwashbuckle.AspNetCore.SwaggerGen;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Program));

        // TODO: do not user DummyCurrentUserService once Authentication is implemented
        services.AddScoped<ICurrentUserService, DummyCurrentUserService>();

        services.AddHttpContextAccessor();

        //services.AddHealthChecks()
        //    .AddDbContextCheck<ApplicationDbContext>();

        services.AddCustomExceptionHandlers();

        services.AddControllers();
        
        // Customize default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Company API", Version = "v1" });
            c.EnableAnnotations();
            c.SupportNonNullableReferenceTypes();
            c.CustomOperationIds(apiDesc => apiDesc.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null);
        });

        services.AddEndpointsApiExplorer();

        return services;
    }
}
