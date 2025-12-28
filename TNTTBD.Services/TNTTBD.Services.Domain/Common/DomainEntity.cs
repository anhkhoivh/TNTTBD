namespace TNTTBD.Services.Domain.Common;

/// <summary>
/// Domain base model
/// </summary>
public abstract class DomainEntity
{
    public Guid Id { get; protected set; }

    protected DomainEntity() { }

    protected DomainEntity(Guid id)
    {
        Id = id;
    }

    public override bool Equals(object obj)
        => obj is DomainEntity other && Id.Equals(other.Id);

    public override int GetHashCode()
        => Id.GetHashCode();
}