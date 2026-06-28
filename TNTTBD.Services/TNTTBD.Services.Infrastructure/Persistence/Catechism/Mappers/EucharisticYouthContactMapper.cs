using TNTTBD.Services.Domain.Catechism.JoinModels;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

/// <summary>
/// User mapper
/// </summary>
public static class EucharisticYouthContactMapper
{
    #region Convert methods from entity to domain model

    /// <summary>
    /// Convert from entity to domain model
    /// </summary>
    public static EucharisticYouthContact ToDomain(this EucharisticYouthContactEntity entity)
    {
        EucharisticYouthContact domain = new EucharisticYouthContact(entity.Id, entity.EucharisticYouthId, entity.ContactId, entity.ContactRelationType);

        return domain;
    }

    #endregion

    #region Convert methods from domain model to entity

    /// <summary>
    /// Convert from domain model to entity
    /// </summary>
    public static EucharisticYouthContactEntity ToEntity(this EucharisticYouthContact domain)
    {
        EucharisticYouthContactEntity entity = new EucharisticYouthContactEntity
        {
            Id = domain.Id,
            ContactId = domain.ContactId,
            EucharisticYouthId = domain.EucharisticYouthId,
            ContactRelationType = domain.ContactRelationType
        };

        return entity;
    }

    /// <summary>
    /// Update entity
    /// </summary>
    public static void UpdateEntity(this EucharisticYouthContact domain, EucharisticYouthContactEntity entity)
    {
        entity.ContactId = domain.ContactId;
        entity.EucharisticYouthId = domain.EucharisticYouthId;
        entity.ContactRelationType = domain.ContactRelationType;
    }

    #endregion
}