namespace Argo.CA.Infrastructure.Configuration;

using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class OptionsExtensions
{
    public static TModel GetRequiredOptions<TModel>(this IConfiguration configuration, string section) 
        where TModel : new()
    {
        var options = configuration
            .GetSection(section)
            .Get<TModel>();

        Guard.Against.Null(
            options,
            section,
            $"Missing configuration '{section}'. Make sure to add a '{section}' section to your appsettings.");

        return options;
    }

    public static IConfiguration GetConfiguration(this IServiceCollection services)
    {
        return services
            .BuildServiceProvider()
            .GetRequiredService<IConfiguration>();
    }
}