namespace TNTTBD.Services.Domain.Common;    

/// <summary>
/// Consistency boundary
/// </summary>
public abstract class AggregateRoot<TId> : DomainEntity<TId>
{
    #region Constructors

    /// <summary>
    /// Constructor from id
    /// </summary>
    /// <param name="id"></param>
    public AggregateRoot(TId id) : base(id) { }

    #endregion

    private readonly List<DomainEvent> _domainEvents = new List<DomainEvent>();
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(DomainEvent domainEvent)
        => _domainEvents.Add(domainEvent);

    protected void ClearDomainEvents()
        => _domainEvents.Clear();
}