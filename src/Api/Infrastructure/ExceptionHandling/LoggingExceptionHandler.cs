using Argo.CA.Domain.Common.Exceptions;

namespace Argo.CA.Api.Infrastructure.ExceptionHandling;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

public class LoggingExceptionHandler(ILogger<LoggingExceptionHandler> logger) : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // always return false for we want execution of exception handlers to continue
        switch (exception)
        {
            case ValidationException validationException:
                logger.LogDebug("A validation exception has occurred. Errors: {errors}", FormatValidationErrors(validationException.Errors));
                return ValueTask.FromResult(false);
            case CustomException:
                logger.LogWarning("A custom exception has occurred: Message: '{message}'.", exception.Message);
                return ValueTask.FromResult(false);
        }

        logger.LogError(exception, "An unhandled exception has occurred.");

        return ValueTask.FromResult(false);
    }

    private static string FormatValidationErrors(IDictionary<string, string[]> validationErrors)
    {
        // Format validation errors for logging
        var formattedErrors = validationErrors.Select(kv => $"{{ {kv.Key}: {string.Join(", ", kv.Value)} }}");
        return string.Join(", ", formattedErrors);
    }
}