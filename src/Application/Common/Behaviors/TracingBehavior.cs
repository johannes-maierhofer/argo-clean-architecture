namespace Argo.CA.Application.Common.Behaviors;

using CQRS;
using MediatR;
using Telemetry;

public class TracingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        using (Telemetry.ActivitySource?.StartActivity(requestName + " send"))
        {
            var result = await next();
            return result;
        }
    }
}