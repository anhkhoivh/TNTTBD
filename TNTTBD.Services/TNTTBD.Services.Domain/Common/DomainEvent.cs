namespace TNTTBD.Services.Domain.Common;

/// <summary>
/// Business events
/// </summary>
public abstract class DomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}