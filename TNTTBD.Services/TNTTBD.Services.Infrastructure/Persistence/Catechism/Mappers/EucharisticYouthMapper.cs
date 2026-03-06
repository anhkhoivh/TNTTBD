using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.ValueObjects;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

/// <summary>
/// User mapper
/// </summary>
public static class EucharisticYouthMapper
{
    #region Convert methods from entity to domain model

    /// <summary>
    /// Convert from entity to domain model
    /// </summary>
    public static EucharisticYouth ToDomain(this EucharisticYouthEntity entity)
    {
        IEnumerable<SpiritualMilestone> milestones = entity.SpiritualMilestones.Select(m => m.ToDomain());
        EucharisticYouth domain = new EucharisticYouth(entity.Id, entity.FullName, entity.DateOfBirth, entity.Address, entity.IsMale, entity.Photos, milestones, entity.PatronSaints);

        return domain;
    }

    #endregion

    #region Convert methods from domain model to entity

    /// <summary>
    /// Convert from domain model to entity
    /// </summary>
    public static EucharisticYouthEntity ToEntity(this EucharisticYouth domain)
    {
        EucharisticYouthEntity entity = new EucharisticYouthEntity
        {
            Id = domain.Id,
            FullName = domain.FullName,
            Address = domain.Address,
            DateOfBirth = domain.DateOfBirth,
            IsMale = domain.IsMale,
            Photos = domain.Photos.ToList(),
            SpiritualMilestones = domain.Milestones.Select(m => m.ToEntity(domain.Id)).ToList(),
            PatronSaints = domain.PatronSaints.ToList()
        };

        return entity;
    }

    /// <summary>
    /// Update entity
    /// </summary>
    public static void UpdateEntity(this EucharisticYouth domain, EucharisticYouthEntity entity)
    {
        entity.FullName = domain.FullName;
        entity.DateOfBirth = domain.DateOfBirth;
        entity.Address = domain.Address;
        entity.IsMale = domain.IsMale;

        entity.Photos.Clear();
        foreach (PhotoReference photo in domain.Photos)
        {
            entity.Photos.Add(photo);
        }

        // UPDATE + ADD
        foreach (SpiritualMilestone milestone in domain.Milestones)
        {
            SpiritualMilestoneEntity? existingEntity = entity.SpiritualMilestones
                .FirstOrDefault(x => x.Id == milestone.Id);

            if (existingEntity == null)
            {
                entity.SpiritualMilestones.Add(milestone.ToEntity(entity.Id));
            }
            else
            {
                milestone.UpdateEntity(existingEntity);
            }
        }

        // REMOVE
        List<SpiritualMilestoneEntity> removedList = entity.SpiritualMilestones
            .Where(x => domain.Milestones.All(m => m.Id != x.Id))
            .ToList();

        foreach (var r in removedList)
        {
            entity.SpiritualMilestones.Remove(r);
        }

        entity.PatronSaints.Clear();
        foreach (PatronSaint patronSaint in domain.PatronSaints)
        {
            entity.PatronSaints.Add(patronSaint);
        }
    }

    #endregion
}