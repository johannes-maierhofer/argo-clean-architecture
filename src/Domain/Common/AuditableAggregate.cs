namespace Argo.CA.Domain.Common;

using System.ComponentModel.DataAnnotations;

public class AuditableAggregate<TId> : Aggregate<TId>, IAuditCreated, IAuditModified
    where TId : struct
{
    public DateTimeOffset CreatedAt { get; private set; }
    
    [MaxLength(100)]
    public string CreatedBy { get; private set; } = string.Empty;

    public DateTimeOffset ModifiedAt { get; private set; }
    
    [MaxLength(100)]
    public string ModifiedBy { get; private set; } = string.Empty;

    public void SetCreated(DateTimeOffset createdAt, string createdBy)
    {
        CreatedAt = createdAt;
        CreatedBy = createdBy;
    }

    public void SetModified(DateTimeOffset modifiedAt, string modifiedBy)
    {
        ModifiedAt = modifiedAt;
        ModifiedBy = modifiedBy;
    }
}