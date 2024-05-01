namespace Argo.CA.Domain.Common;

public interface IAuditCreated
{
    public DateTimeOffset CreatedAt { get; }
    public string CreatedBy { get; }
    public void SetCreated(DateTimeOffset createdAt, string createdBy);
}