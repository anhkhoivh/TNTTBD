namespace TNTTBD.Services.Domain.Common;

/// <summary>
/// Domain base model
/// </summary>
public abstract class DomainEntity<TId>
{
    #region Constructors

    /// <summary>
    /// Constructor from ID
    /// </summary>
    /// <param name="id"></param>
    protected DomainEntity(TId id)
    {
        Id = id;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Id
    /// </summary>
    public TId Id { get; protected set; }

    #endregion

    /// <summary>
    /// Equal method
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj)
    {
        if (obj is not DomainEntity<TId> other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return EqualityComparer<TId>.Default.Equals(Id, other.Id);
    }

    /// <summary>
    /// Hash code
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
        => EqualityComparer<TId>.Default.GetHashCode(Id!);
}