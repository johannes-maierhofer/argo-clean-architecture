namespace Argo.CA.Infrastructure.Persistence.Interceptors;

using Domain.Common;
using Domain.Common.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

public class DomainEventDispatchingInterceptor(IDomainEventPublisher domainEventPublisher) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        return SavingChangesAsync(eventData, result)
            .GetAwaiter()
            .GetResult();
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, 
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        if (eventData.Context == null) return result;

        var dbContext = eventData.Context;
        await DispatchDomainEvents(dbContext);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private async Task DispatchDomainEvents(DbContext? context)
    {
        if (context == null) return;

        var domainEvents = context.ChangeTracker
            .Entries<IHasDomainEvents>()
            .Select(e => e.Entity)
            .SelectMany(e => e.PopDomainEvents())
            .ToList();

        foreach (var domainEvent in domainEvents)
            await domainEventPublisher.Publish(domainEvent);
    }
}