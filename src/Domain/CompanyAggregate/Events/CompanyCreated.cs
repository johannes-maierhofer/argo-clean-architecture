namespace Argo.CA.Domain.CompanyAggregate.Events;

using Common.Events;

public record CompanyCreated(Company Company) : IDomainEvent;