namespace Argo.CA.Domain.Common;

using Events;

public interface IHasDomainEvents
{
    List<IDomainEvent> PopDomainEvents();
}