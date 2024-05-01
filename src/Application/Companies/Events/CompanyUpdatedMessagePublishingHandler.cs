namespace Argo.CA.Application.Companies.Events
{
    using Domain.CompanyAggregate.Events;
    using MediatR;
    using Microsoft.Extensions.Logging;

    public class CompanyUpdatedMessagePublishingHandler(ILogger<CompanyUpdated> logger)
        : INotificationHandler<CompanyUpdated>
    {
        public Task Handle(CompanyUpdated domainEvent, CancellationToken cancellationToken)
        {
            // not sure what to do when company Email changed
            logger.LogInformation("Handling CompanyUpdated event for company with id {CompanyId}",
                domainEvent.Company.Id);

            return Task.CompletedTask;
        }
    }
}
