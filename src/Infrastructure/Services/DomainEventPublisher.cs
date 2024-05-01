namespace Argo.CA.Infrastructure.Services;

using Domain.Common.Events;
using MediatR;

public class DomainEventPublisher(IMediator mediator) : IDomainEventPublisher
{
    public async Task Publish(IDomainEvent domainEvent)
    {
        await mediator.Publish(domainEvent);
    }
}