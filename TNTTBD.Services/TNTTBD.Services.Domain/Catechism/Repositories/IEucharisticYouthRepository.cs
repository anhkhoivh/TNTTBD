using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Domain.Catechism.Repositories;

public interface IEucharisticYouthRepository
{
    /// <summary>
    /// Get Eucharistic Youth by id
    /// </summary>
    Task<EucharisticYouth?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Add new Eucharistic Youth
    /// </summary>
    Task AddAsync(EucharisticYouth youth, CancellationToken cancellationToken);

    /// <summary>
    /// Add milestone
    /// </summary>
    Task AddMilestone(Guid id, SpiritualMilestone milestone, CancellationToken cancellationToken);

    /// <summary>
    /// Add photo
    /// </summary>
    Task AddPhoto(Guid id, PhotoReference photoReference, CancellationToken cancellationToken);

    /// <summary>
    /// Add Patron Saint
    /// </summary>
    Task AddPatronSaint(Guid id, PatronSaint patronSaint, CancellationToken cancellationToken);

    /// <summary>
    /// Update milestone
    /// </summary>
    Task UpdateMilestone(Guid id, SpiritualMilestone milestone, CancellationToken cancellationToken);

    /// <summary>
    /// Remove milestone
    /// </summary>
    Task RemoveMilestone(Guid id, Guid milestoneId, CancellationToken cancellationToken);

    /// <summary>
    /// Add Patron Saint
    /// </summary>
    Task<string> RemovePhoto(Guid id, Guid photoId, CancellationToken cancellationToken);
}