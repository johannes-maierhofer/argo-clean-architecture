namespace Argo.CA.Domain.CompanyAggregate.Events;

using Common.Events;

public record CompanyUpdated(Company Company) : IDomainEvent;