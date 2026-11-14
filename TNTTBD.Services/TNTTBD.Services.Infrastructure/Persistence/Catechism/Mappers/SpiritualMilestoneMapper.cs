using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

/// <summary>
/// Spiritual milestone mapper
/// </summary>
public static class SpiritualMilestoneMapper
{
    #region Convert methods from entity to domain model

    /// <summary>
    /// Convert from entity to domain model
    /// </summary>
    public static SpiritualMilestone ToDomain(this SpiritualMilestoneEntity entity)
    {
        return new SpiritualMilestone(
            entity.Id,
            entity.Type,
            entity.Date,
            entity.DocumentCode,
            entity.Place,
            entity.Note,
            entity.SponsorContactId,
            entity.Celebrant,
            entity.MilestoneEventId);
    }

    #endregion

    #region Convert methods from domain model to entity

    /// <summary>
    /// Convert from domain model to entity
    /// </summary>
    public static SpiritualMilestoneEntity ToEntity(this SpiritualMilestone domain, Guid eucharisticYouthId)
    {
        SpiritualMilestoneEntity entity = new SpiritualMilestoneEntity
        {
            Id = domain.Id,
            Type = domain.Type,
            EucharisticYouthId = eucharisticYouthId
        };
        domain.UpdateEntity(entity);
        return entity;
    }

    /// <summary>
    /// Update entity
    /// </summary>
    public static void UpdateEntity(this SpiritualMilestone domain, SpiritualMilestoneEntity entity)
    {
        entity.Date = domain.Date;
        entity.DocumentCode = domain.DocumentCode;
        entity.Type = domain.Type;
        entity.Note = domain.Note;
        entity.Place = domain.Place;
        entity.SponsorContactId = domain.SponsorContactId;
        entity.Celebrant = domain.Celebrant;
        entity.MilestoneEventId = domain.MilestoneEventId;
    }

    #endregion
}
