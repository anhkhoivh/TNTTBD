using TNTTBD.Services.Domain.Catechism.JoinModels;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Domain.Catechism.Repositories;

public interface ICatechismClassEucharisticYouthRepository
{
    /// <summary>
    /// Add 
    /// /// </summary>
    Task AddAsync(CatechismClassEucharisticYouth domain, CancellationToken cancellationToken);

    /// <summary>
    /// Remove 
    /// </summary>
    Task Remove(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Update an element
    /// </summary>
    /// <param name="id"></param>
    /// <param name="status"></param>
    /// <param name="note"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Update(Guid id, EnrollmentStatus status, string note, CancellationToken cancellationToken);

    /// <summary>
    /// Update collection
    /// </summary>
    Task MarkCompleted(IEnumerable<Guid> ids, CancellationToken cancellationToken);

    Task<List<Guid>> GetActiveEnrollmentIdsAsync(Guid classId, IEnumerable<Guid> youthIds, CancellationToken cancellationToken);
}