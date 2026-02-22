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
        CatechismClassEucharisticYouth domain = new CatechismClassEucharisticYouth(entity.Id, entity.EucharisticYouthId, entity.CatechismClassId);

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

    #endregion
}