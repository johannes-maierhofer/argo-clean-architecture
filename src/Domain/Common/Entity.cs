namespace Argo.CA.Domain.Common;

public abstract class Entity<TId>
    where TId : struct
{
    protected Entity(TId id) => Id = id;
    protected Entity() { }

    public TId Id { get; protected init; }

    public override bool Equals(object? other)
    {
        if (other is null || other.GetType() != GetType())
        {
            return false;
        }

        return ((Entity<TId>)other).Id.Equals(Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

}