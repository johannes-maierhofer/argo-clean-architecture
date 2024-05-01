namespace Argo.CA.Domain.Common;

public interface IAuditModified
{
    public DateTimeOffset ModifiedAt { get; }
    public string ModifiedBy { get; }
    public void SetModified(DateTimeOffset modifiedAt, string modifiedBy);
}