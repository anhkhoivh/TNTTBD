namespace TNTTBD.Services.Domain.Catechism.Repositories;

public interface ICatechismSessionRepository
{
    /// <summary>
    /// Add session
    /// </summary>
    Task AddAsync(CatechismSession catechismSession, CancellationToken cancellationToken);

    /// <summary>
    /// Update session
    /// </summary>
    Task Update(Guid id, DateTimeOffset dateTime, string title, CancellationToken cancellationToken);

    /// <summary>
    /// Remove session
    /// </summary>
    Task Remove(Guid id, CancellationToken cancellationToken);
}