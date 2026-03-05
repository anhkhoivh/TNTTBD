using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.ValueObjects;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

/// <summary>
/// User mapper
/// </summary>
public static class CatechistMapper
{
    #region Convert methods from entity to domain model

    /// <summary>
    /// Convert from entity to domain model
    /// </summary>
    public static Catechist ToDomain(this CatechistEntity entity)
    {
        IEnumerable<SpiritualMilestone> milestones = entity.SpiritualMilestones.Select(m => m.ToDomain());
        Catechist domain = new Catechist(entity.Id, entity.FullName, entity.DateOfBirth, entity.Address, entity.IsMale, entity.Photos, milestones, entity.PatronSaints);
        return domain;
    }

    #endregion

    #region Convert methods from domain model to entity

    /// <summary>
    /// Convert from domain model to entity
    /// </summary>
    public static CatechistEntity ToEntity(this Catechist domain)
    {
        CatechistEntity entity = new CatechistEntity
        {
            Id = domain.Id,
            FullName = domain.FullName,
            Address = domain.Address,
            DateOfBirth = domain.DateOfBirth,
            IsMale = domain.IsMale,
            SpiritualMilestones = domain.Milestones.Select(m => m.ToEntity(domain.Id)).ToList(),
        };

        entity.Photos.Clear();
        foreach (PhotoReference photoReference in domain.Photos)
        {
            entity.Photos.Add(photoReference);
        }

        entity.PatronSaints.Clear();
        foreach (PatronSaint patronSaint in domain.PatronSaints)
        {
            entity.PatronSaints.Add(patronSaint);
        }

        return entity;
    }

    /// <summary>
    /// Update entity
    /// </summary>
    public static void UpdateEntity(this Catechist domain, CatechistEntity entity)
    {
        entity.FullName = domain.FullName;
        entity.DateOfBirth = domain.DateOfBirth;
        entity.Address = domain.Address;
        entity.IsMale = domain.IsMale;

        entity.Photos.Clear();
        foreach (PhotoReference photoReference in domain.Photos)
        {
            entity.Photos.Add(photoReference);
        }

        entity.SpiritualMilestones.Clear();
        entity.SpiritualMilestones = domain.Milestones.Select(m => m.ToEntity(domain.Id)).ToList();

        entity.PatronSaints.Clear();
        foreach (PatronSaint patronSaint in domain.PatronSaints)
        {
            entity.PatronSaints.Add(patronSaint);
        }
    }

    #endregion
}