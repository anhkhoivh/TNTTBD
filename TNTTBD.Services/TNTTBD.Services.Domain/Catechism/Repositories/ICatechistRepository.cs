using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Domain.Catechism.Repositories;

public interface ICatechistRepository
{
    /// <summary>
    /// Get catechist by id
    /// </summary>
    Task<Catechist?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Add new catechist
    /// </summary>
    Task AddAsync(Catechist catechist, CancellationToken cancellationToken);

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
    Task Update(Guid id, FullName fullName, DateOnly dateOfBirth, string? address, bool isMale, CancellationToken cancellationToken);

    /// <summary>
    /// Remove
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Remove(Guid id, CancellationToken cancellationToken);

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
    /// Add Patron Saint
    /// </summary>
    Task<string> RemovePhoto(Guid id, Guid photoId, CancellationToken cancellationToken);
}