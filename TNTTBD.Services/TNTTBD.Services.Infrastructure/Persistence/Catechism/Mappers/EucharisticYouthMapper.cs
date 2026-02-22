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
        EucharisticYouth domain = new EucharisticYouth(entity.Id, entity.FullName, entity.DateOfBirth, entity.Address, entity.Photos, entity.SpiritualMilestones, entity.PatronSaints);

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
            Photos = domain.Photos.ToList(),
            SpiritualMilestones = domain.Milestones.ToList(),
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

        entity.Photos.Clear();
        foreach (PhotoReference photo in domain.Photos)
        {
            entity.Photos.Add(photo);
        }

        entity.SpiritualMilestones.Clear();
        foreach (SpiritualMilestone milestone in domain.Milestones)
        {
            entity.SpiritualMilestones.Add(milestone);
        }

        entity.PatronSaints.Clear();
        foreach (PatronSaint patronSaint in domain.PatronSaints)
        {
            entity.PatronSaints.Add(patronSaint);
        }
    }

    #endregion
}