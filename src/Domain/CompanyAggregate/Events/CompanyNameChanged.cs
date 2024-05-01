namespace Argo.CA.Domain.CompanyAggregate.Events;

using Common.Events;

public record CompanyNameChanged(
    Guid CompanyId,
    string Name,
    string OldName): IDomainEvent;