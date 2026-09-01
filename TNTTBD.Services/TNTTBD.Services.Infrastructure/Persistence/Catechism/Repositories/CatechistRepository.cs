using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.Common;
using TNTTBD.Services.Domain.ValueObjects;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Repositories;

public class CatechistRepository : ICatechistRepository
{
    #region Fields

    private readonly CatechismDbContext _context;

    #endregion

    #region Constructors

    public CatechistRepository(CatechismDbContext context)
    {
        _context = context;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public async Task AddAsync(Catechist catechist, CancellationToken cancellationToken)
    {
        CatechistEntity entity = catechist.ToEntity();
        await _context.Catechists.AddAsync(entity, cancellationToken);
    }

    /// <inheritdoc />
    public async Task Update(Guid id, FullName fullName, DateOnly dateOfBirth, string? address, bool isMale, IReadOnlyCollection<PatronSaint>? patronSaints, CancellationToken cancellationToken)
    {
        CatechistEntity? catechistEntity = await _context.Catechists.FirstOrDefaultAsync(y => y.Id == id, cancellationToken);
        if (catechistEntity == null)
        {
            throw new DomainException(DomainErrors.ValidationNotFound);
        }

        Catechist domain = catechistEntity.ToDomain();
        domain.UpdateInfo(fullName, dateOfBirth, address, isMale, patronSaints);
        domain.UpdateEntity(catechistEntity);
    }

    /// <inheritdoc />
    public async Task Remove(Guid id, CancellationToken cancellationToken)
    {
        CatechistEntity? catechistEntity = await _context.Catechists
            .FirstOrDefaultAsync(y => y.Id == id, cancellationToken);
        if (catechistEntity == null)
        {
            throw new DomainException(DomainErrors.ValidationNotFound);
        }
        _context.Catechists.Remove(catechistEntity);
    }

    /// <inheritdoc />
    public async Task AddPhoto(Guid id, PhotoReference photoReference, CancellationToken cancellationToken)
    {
        CatechistEntity? catechistEntity = await _context.Catechists
            .Include(y => y.Photos)
            .FirstOrDefaultAsync(y => y.Id == id, cancellationToken);
        if (catechistEntity == null)
        {
            return;
        }
        Catechist domain = catechistEntity.ToDomain();
        domain.AddPhoto(photoReference);
        domain.UpdateEntity(catechistEntity);
    }

    /// <inheritdoc />
    public async Task<string> RemovePhoto(Guid id, Guid photoId, CancellationToken cancellationToken)
    {
        CatechistEntity? catechistEntity = await _context.Catechists
            .Include(y => y.Photos)
            .FirstOrDefaultAsync(y => y.Id == id, cancellationToken);
        if (catechistEntity == null)
        {
            throw new DomainException(DomainErrors.ValidationNotFound);
        }
        Catechist domain = catechistEntity.ToDomain();
        PhotoReference photoReference = domain.RemovePhoto(photoId);
        domain.UpdateEntity(catechistEntity);
        return photoReference.StorageKey;
    }

    /// <inheritdoc />
    public async Task AddPatronSaint(Guid id, PatronSaint patronSaint, CancellationToken cancellationToken)
    {
        CatechistEntity? catechistEntity = await _context.Catechists
            .Include(y => y.PatronSaints)
            .FirstOrDefaultAsync(y => y.Id == id, cancellationToken);
        if (catechistEntity == null)
        {
            return;
        }
        Catechist domain = catechistEntity.ToDomain();
        domain.AddPatronSaint(patronSaint);
        domain.UpdateEntity(catechistEntity);
    }

    /// <inheritdoc />
    public async Task RemovePatronSaint(Guid id, PatronSaint patronSaint, CancellationToken cancellationToken)
    {
        CatechistEntity? catechistEntity = await _context.Catechists
            .Include(y => y.PatronSaints)
            .FirstOrDefaultAsync(y => y.Id == id, cancellationToken);
        if (catechistEntity == null)
        {
            throw new DomainException(DomainErrors.ValidationNotFound);
        }
        Catechist domain = catechistEntity.ToDomain();
        domain.RemovePatronSaint(patronSaint);
        domain.UpdateEntity(catechistEntity);
    }

    #endregion
}