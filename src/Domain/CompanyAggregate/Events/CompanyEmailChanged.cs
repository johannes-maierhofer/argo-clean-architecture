namespace Argo.CA.Domain.CompanyAggregate.Events;

using Common.Events;

public record CompanyEmailChanged(
    Guid CompanyId,
    string Email,
    string OldEmail): IDomainEvent;