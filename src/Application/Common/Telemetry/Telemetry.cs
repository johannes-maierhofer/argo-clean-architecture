namespace Argo.CA.Application.Common.Telemetry;

using System.Diagnostics;

/*
 * ref. https://learn.microsoft.com/en-us/dotnet/core/diagnostics/distributed-tracing-instrumentation-walkthroughs
 * Create the ActivitySource once, store it in a static variable and use that instance as long as needed.
 * Each library or library sub-component can (and often should) create its own source.
 */
public static class Telemetry
{
    // Name it after the service name for your app.
    // It can come from a config file, constants file, etc.
    public static ActivitySource? ActivitySource;
}