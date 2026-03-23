using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.Common;
using TNTTBD.Services.Domain.ValueObjects;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Repositories;

public class EucharisticYouthRepository : IEucharisticYouthRepository
{
    private readonly CatechismDbContext _context;

    public EucharisticYouthRepository(CatechismDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task AddAsync(
        EucharisticYouth youth,
        CancellationToken cancellationToken)
    {
        EucharisticYouthEntity entity = youth.ToEntity();
        await _context.EucharisticYouths.AddAsync(entity, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<EucharisticYouth?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        EucharisticYouthEntity? entity = await _context.EucharisticYouths
            .Include(x => x.Photos)
            .Include(x => x.SpiritualMilestones)
            .Include(x => x.PatronSaints)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity?.ToDomain();
    }

    /// <inheritdoc />
    public async Task Update(Guid id, FullName fullName, DateOnly dateOfBirth, string? address, bool isMale, CancellationToken cancellationToken)
    {
        EucharisticYouthEntity? youthEntity = await _context.EucharisticYouths.FirstOrDefaultAsync(y => y.Id == id, cancellationToken);
        if (youthEntity == null)
        {
            throw new Exception(DomainErrors.ValidationNotFound);
        }

        EucharisticYouth domain = youthEntity.ToDomain();
        domain.UpdateInfo(fullName, dateOfBirth, address, isMale);
        domain.UpdateEntity(youthEntity);
    }

    /// <inheritdoc />
    public async Task Remove(Guid id, CancellationToken cancellationToken)
    {
        EucharisticYouthEntity? youthEntity = await _context.EucharisticYouths
            .FirstOrDefaultAsync(y => y.Id == id, cancellationToken);
        if (youthEntity == null)
        {
            throw new Exception(DomainErrors.ValidationNotFound);
        }
        _context.Remove(youthEntity);
    }

    /// <inheritdoc />
    public async Task AddMilestone(Guid id, SpiritualMilestone milestone, CancellationToken cancellationToken)
    {
        EucharisticYouthEntity? youthEntity = await _context.EucharisticYouths
            .Include(y => y.SpiritualMilestones)
            .FirstOrDefaultAsync(y => y.Id == id, cancellationToken);
        if (youthEntity == null)
        {
            throw new Exception(DomainErrors.ValidationNotFound);
        }
        EucharisticYouth domain = youthEntity.ToDomain();
        domain.AddMilestone(milestone);
        domain.UpdateEntity(youthEntity);
    }

    /// <inheritdoc />
    public async Task UpdateMilestone(Guid id, SpiritualMilestone milestone, CancellationToken cancellationToken)
    {
        EucharisticYouthEntity? youthEntity = await _context.EucharisticYouths
            .Include(y => y.SpiritualMilestones)
            .FirstOrDefaultAsync(y => y.Id == id, cancellationToken);
        if (youthEntity == null)
        {
            throw new Exception(DomainErrors.ValidationNotFound);
        }
        SpiritualMilestoneEntity? milestoneEntity = youthEntity.SpiritualMilestones
            .FirstOrDefault(m => m.Id == milestone.Id);
        if (milestoneEntity == null)
        {
            throw new Exception(DomainErrors.ValidationNotFound);
        }
        EucharisticYouth domain = youthEntity.ToDomain();
        domain.UpdateMilestone(milestone.Id, milestone.Date, milestone.DocumentCode, milestone.Place, milestone.Note);
        domain.UpdateEntity(youthEntity);
    }  

    /// <inheritdoc />
    public async Task RemoveMilestone(Guid id, Guid milestoneId, CancellationToken cancellationToken)
    {
        EucharisticYouthEntity? youthEntity = await _context.EucharisticYouths
            .Include(y => y.SpiritualMilestones)
            .FirstOrDefaultAsync(y => y.Id == id, cancellationToken);
        if (youthEntity == null)
        {
            throw new Exception(DomainErrors.ValidationNotFound);
        }
        EucharisticYouth domain = youthEntity.ToDomain();
        domain.RemoveMilestone(milestoneId);
        domain.UpdateEntity(youthEntity);
    }

    /// <inheritdoc />
    public async Task AddPhoto(Guid id, PhotoReference photoReference, CancellationToken cancellationToken)
    {
        EucharisticYouthEntity? youthEntity = await _context.EucharisticYouths
            .Include(y => y.Photos)
            .FirstOrDefaultAsync(y => y.Id == id, cancellationToken);
        if (youthEntity == null)
        {
            throw new Exception(DomainErrors.ValidationNotFound);
        }
        EucharisticYouth domain = youthEntity.ToDomain();
        domain.AddPhoto(photoReference);
        domain.UpdateEntity(youthEntity);
    }

    /// <inheritdoc />
    public async Task<string> RemovePhoto(Guid id, Guid photoId, CancellationToken cancellationToken)
    {
        EucharisticYouthEntity? youthEntity = await _context.EucharisticYouths
            .Include(y => y.Photos)
            .FirstOrDefaultAsync(y => y.Id == id, cancellationToken);
        if (youthEntity == null)
        {
            throw new Exception(DomainErrors.ValidationNotFound);
        }
        EucharisticYouth domain = youthEntity.ToDomain();
        PhotoReference photoReference = domain.RemovePhoto(photoId);
        domain.UpdateEntity(youthEntity);
        return photoReference.StorageKey;
    }

    /// <inheritdoc />
    public async Task AddPatronSaint(Guid id, PatronSaint patronSaint, CancellationToken cancellationToken)
    {
        EucharisticYouthEntity? youthEntity = await _context.EucharisticYouths
            .Include(y => y.PatronSaints)
            .FirstOrDefaultAsync(y => y.Id == id, cancellationToken);
        if (youthEntity == null)
        {
            throw new Exception(DomainErrors.ValidationNotFound);
        }
        EucharisticYouth domain = youthEntity.ToDomain();
        domain.AddPatronSaint(patronSaint);
        domain.UpdateEntity(youthEntity);
    }

    /// <inheritdoc />
    public async Task RemovePatronSaint(Guid id, PatronSaint patronSaint, CancellationToken cancellationToken)
    {
        EucharisticYouthEntity? youthEntity = await _context.EucharisticYouths
            .Include(y => y.PatronSaints)
            .FirstOrDefaultAsync(y => y.Id == id, cancellationToken);
        if (youthEntity == null)
        {
            throw new Exception(DomainErrors.ValidationNotFound);
        }
        EucharisticYouth domain = youthEntity.ToDomain();
        domain.RemovePatronSaint(patronSaint);
        domain.UpdateEntity(youthEntity);
    }
}