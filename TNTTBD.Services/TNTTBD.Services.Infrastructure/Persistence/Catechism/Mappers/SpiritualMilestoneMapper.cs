using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Domain.Catechism;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

/// <summary>
/// User mapper
/// </summary>
public static class SpiritualMilestoneMapper
{
    #region Convert methods from entity to domain model

    /// <summary>
    /// Convert from entity to domain model
    /// </summary>
    public static SpiritualMilestone ToDomain(this SpiritualMilestoneEntity entity)
    {
        SpiritualMilestone domain = new(entity.Id, entity.Type, entity.Date, entity.DocumentCode, entity.Place, entity.Note);
        return domain;
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
            Date = domain.Date,
            DocumentCode = domain.DocumentCode,
            Note = domain.Note,
            Place = domain.Place,
            Type = domain.Type,
            EucharisticYouthId = eucharisticYouthId
        };

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
    }

    #endregion

    #region Convert methods from entity to DTO

    /// <summary>
    /// Convert from entity to DTO
    /// </summary>
    public static SpiritualMilestoneDto ToDTO(this SpiritualMilestoneEntity entity)
    {
        return new SpiritualMilestoneDto
            {
                Type = entity.Type.ToString(),
                Date = entity.Date,
                Place = entity.Place,
                Note = entity.Note
            };
    }

    #endregion
}