using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.Enums;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Domain.Catechism.Repositories;

public interface IEucharisticYouthRepository
{
    /// <summary>
    /// Add new Eucharistic Youth
    /// </summary>
    Task AddAsync(EucharisticYouth youth, CancellationToken cancellationToken);

    /// <summary>
    /// Update basic info
    /// </summary>
    /// <param name="id"></param>
    /// <param name="fullName"></param>
    /// <param name="dateOfBirth"></param>
    /// <param name="address"></param>
    /// <param name="isMale"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Update(Guid id, FullName fullName, DateOnly dateOfBirth, string? address, bool isMale, IReadOnlyCollection<PatronSaint>? patronSaints, IReadOnlyCollection<SpiritualMilestone>? milestones, CancellationToken cancellationToken);

    /// <summary>
    /// Remove Eucharistic Youth
    /// </summary>
    Task Remove(Guid id, CancellationToken cancellationToken);

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
    /// Remove Patron Saint
    /// </summary>
    Task RemovePatronSaint(Guid id, PatronSaint patronSaint, CancellationToken cancellationToken);

    /// <summary>
    /// Update milestone
    /// </summary>
    /// <returns>The type of the updated milestone</returns>
    Task<MilestoneType> UpdateMilestone(Guid id, SpiritualMilestone milestone, CancellationToken cancellationToken);

    /// <summary>
    /// Remove milestone
    /// </summary>
    Task RemoveMilestone(Guid id, Guid milestoneId, CancellationToken cancellationToken);

    /// <summary>
    /// Update photo purpose
    /// </summary>
    Task UpdatePhoto(Guid id, Guid photoId, PhotoPurpose purpose, CancellationToken cancellationToken);

    /// <summary>
    /// Remove photo and return its storage key
    /// </summary>
    Task<string> RemovePhoto(Guid id, Guid photoId, CancellationToken cancellationToken);
}