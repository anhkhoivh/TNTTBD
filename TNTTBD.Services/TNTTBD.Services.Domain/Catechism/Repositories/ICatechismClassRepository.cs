namespace TNTTBD.Services.Domain.Catechism.Repositories;

public interface ICatechismClassRepository
{
    /// <summary>
    /// Add class
    /// </summary>
    Task AddAsync(CatechismClass catechismClass, CancellationToken cancellationToken);

    /// <summary>
    /// Update class info
    /// </summary>
    Task Update(Guid id, string className, string classCode, CancellationToken cancellationToken);

    /// <summary>
    /// Remove class
    /// </summary>
    Task Remove(Guid id, CancellationToken cancellationToken);
}