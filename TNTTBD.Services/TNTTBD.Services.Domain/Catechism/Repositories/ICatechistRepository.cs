using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Domain.Catechism.Repositories;

public interface ICatechistRepository
{
    /// <summary>
    /// Get Eucharistic Youth by id
    /// </summary>
    Task<Catechist?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Add new Eucharistic Youth
    /// </summary>
    Task AddAsync(Catechist catechist, CancellationToken cancellationToken);

    /// <summary>
    /// Add photo
    /// </summary>
    Task AddPhoto(Guid id, PhotoReference photoReference, CancellationToken cancellationToken);

    /// <summary>
    /// Add Patron Saint
    /// </summary>
    Task AddPatronSaint(Guid id, PatronSaint patronSaint, CancellationToken cancellationToken);
}