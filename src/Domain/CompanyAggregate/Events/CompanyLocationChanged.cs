namespace Argo.CA.Domain.CompanyAggregate.Events;

using Common.Events;
using ValueObjects;

public record CompanyLocationChanged(
    Guid CompanyId,
    CompanyLocation Location,
    CompanyLocation OldLocation) : IDomainEvent;