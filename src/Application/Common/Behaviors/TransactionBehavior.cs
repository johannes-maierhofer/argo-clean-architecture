namespace Argo.CA.Application.Common.Behaviors;

using CQRS;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

public class TransactionBehavior<TRequest, TResponse>(IAppDbContext dbContext)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand
    where TResponse : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // for Connection Resiliency
        // see https://learn.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency

        var response = default(TResponse);

        var strategy = dbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(
            async () =>
            {
                await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

                response = await next();

                await transaction.CommitAsync(cancellationToken);
            });
            
        return response!;
    }
}