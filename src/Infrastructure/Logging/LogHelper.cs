namespace Argo.CA.Infrastructure.Logging;

using Microsoft.AspNetCore.Http;
using Serilog.Events;

public static class LogHelper
{
    public static LogEventLevel GetCustomLogEventLevel(HttpContext ctx, double _, Exception? ex)
    {
        if (ex != null)
            return LogEventLevel.Error;

        if (ctx.Response.StatusCode > 499)
            return LogEventLevel.Error;

        if (IsHealthCheckEndpoint(ctx))
            return LogEventLevel.Verbose;

        return LogEventLevel.Information;
    }

    private static bool IsHealthCheckEndpoint(HttpContext ctx)
    {
        var isHealthCheck = ctx.Request.Path == "/healthz";
        return isHealthCheck;
    }
}