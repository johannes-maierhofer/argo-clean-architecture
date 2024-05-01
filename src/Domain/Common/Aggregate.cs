namespace Argo.CA.Domain.Common;

using Events;

public abstract class Aggregate<TId> : Entity<TId>, IHasDomainEvents
    where TId : struct
{
    protected readonly List<IDomainEvent> DomainEvents = new();

    public List<IDomainEvent> PopDomainEvents()
    {
        var dequeuedEvents = DomainEvents.ToList();

        DomainEvents.Clear();

        return dequeuedEvents;
    }
}
