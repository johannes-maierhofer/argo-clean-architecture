using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Argo.CA.Api.IntegrationTests.Testing.Authentication;

// see https://github.com/FrodeHus/testing-jwt-apps/tree/main

public static class ConfigurationExtensions
{
    public static IServiceCollection AddTestJwtBearerAuthentication(this IServiceCollection services)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        services.Configure<JwtBearerOptions>(
            JwtBearerDefaults.AuthenticationScheme,
            options =>
            {
                options.Configuration = new OpenIdConnectConfiguration
                {
                    Issuer = TestJwtTokenProvider.Issuer
                };
                options.TokenValidationParameters.ValidIssuer = TestJwtTokenProvider.Issuer;
                options.TokenValidationParameters.ValidAudience = TestJwtTokenProvider.Issuer;
                options.Configuration.SigningKeys.Add(TestJwtTokenProvider.SecurityKey);
            }
        );

        return services;
    }
}