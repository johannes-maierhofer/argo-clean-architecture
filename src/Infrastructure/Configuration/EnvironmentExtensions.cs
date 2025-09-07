using Ardalis.GuardClauses;
using Microsoft.Extensions.Hosting;

namespace Argo.CA.Infrastructure.Configuration;

public static class EnvironmentExtensions
{
    public static bool IsTesting(this IHostEnvironment hostEnvironment)
    {
        Guard.Against.Null(hostEnvironment);

        return hostEnvironment.IsEnvironment(CustomEnvironments.Testing);
    }
}