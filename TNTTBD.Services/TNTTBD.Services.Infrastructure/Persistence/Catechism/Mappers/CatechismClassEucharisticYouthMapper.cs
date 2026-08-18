using TNTTBD.Services.Application.CatechismClassEucharisticYouths.DTOs;
using TNTTBD.Services.Domain.Catechism.JoinModels;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

/// <summary>
/// User mapper
/// </summary>
public static class CatechismClassEucharisticYouthMapper
{
    #region Convert methods from entity to domain model

    /// <summary>
    /// Convert from entity to domain model
    /// </summary>
    public static CatechismClassEucharisticYouth ToDomain(this CatechismClassEucharisticYouthEntity entity)
    {
        CatechismClassEucharisticYouth domain = new CatechismClassEucharisticYouth(
            entity.Id, entity.EucharisticYouthId, entity.CatechismClassId,
            entity.EnrolledAt, entity.LeftAt, entity.Status, entity.Note);

        return domain;
    }

    #endregion

    #region Convert methods from domain model to entity

    /// <summary>
    /// Convert from domain model to entity
    /// </summary>
    public static CatechismClassEucharisticYouthEntity ToEntity(this CatechismClassEucharisticYouth domain)
    {
        CatechismClassEucharisticYouthEntity entity = new CatechismClassEucharisticYouthEntity
        {
            Id = domain.Id,
            CatechismClassId = domain.CatechismClassId,
            EucharisticYouthId = domain.EucharisticYouthId,
            EnrolledAt = domain.EnrolledAt,
            LeftAt = domain.LeftAt,
            Status = domain.Status,
            Note = domain.Note,
        };

        return entity;
    }

    /// <summary>
    /// Update entity
    /// </summary>
    public static void UpdateEntity(this CatechismClassEucharisticYouth domain, CatechismClassEucharisticYouthEntity entity)
    {
        entity.CatechismClassId = domain.CatechismClassId;
        entity.EucharisticYouthId = domain.EucharisticYouthId;
    }

    /// <summary>
    /// Convert from entity to DTO
    /// </summary>
    public static CatechismClassEucharisticYouthDto ToDTO(this CatechismClassEucharisticYouthEntity entity)
    {
        return new CatechismClassEucharisticYouthDto
            {
                Id = entity.Id,
                CatechismClassId = entity.CatechismClassId,
                ClassName = entity.CatechismClass.ClassName,
                CatechismYearTheme = entity.CatechismClass.CatechismYear.Theme ?? string.Empty,
                EucharisticYouthId = entity.EucharisticYouthId,
                Code = entity.EucharisticYouth.Code,
                FullName = entity.EucharisticYouth.FullName,
                EnrolledAt = entity.EnrolledAt,
                LeftAt = entity.LeftAt,
                Status = entity.Status,
                Note = entity.Note,
            };
    }

    #endregion
}