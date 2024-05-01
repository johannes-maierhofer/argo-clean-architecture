namespace Argo.CA.Domain.Common.Events;

public interface IDomainEventPublisher
{
    Task Publish(IDomainEvent domainEvent);
}