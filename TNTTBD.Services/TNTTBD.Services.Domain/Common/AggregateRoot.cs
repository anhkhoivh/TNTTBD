namespace TNTTBD.Services.Domain.Common;    

/// <summary>
/// Consistency boundary
/// </summary>
public abstract class AggregateRoot : DomainEntity
{
    private readonly List<DomainEvent> _domainEvents = new List<DomainEvent>();
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(DomainEvent domainEvent)
        => _domainEvents.Add(domainEvent);

    protected void ClearDomainEvents()
        => _domainEvents.Clear();
}