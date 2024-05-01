namespace Argo.CA.Application.Companies.Events
{
    using Domain.CompanyAggregate.Events;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class CompanyCreatedMessagePublishingHandler(ILogger<CompanyCreatedMessagePublishingHandler> logger)
        : INotificationHandler<CompanyCreated>
    {
        public Task Handle(CompanyCreated domainEvent, CancellationToken cancellationToken)
        {
            logger.LogInformation("Handling CompanyCreated event for company with id {CompanyId}",
                domainEvent.Company.Id);

            return Task.CompletedTask;
        }
    }
}
