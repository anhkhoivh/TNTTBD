using TNTTBD.Services.Domain.Catechism;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

/// <summary>
/// User mapper
/// </summary>
public static class EucharisticYouthRunningNumberMapper
{
    #region Convert methods from entity to domain model

    /// <summary>
    /// Convert from entity to domain model
    /// </summary>
    public static EucharisticYouthRunningNumber ToDomain(this EucharisticYouthRunningNumberEntity entity)
    {
        EucharisticYouthRunningNumber domain = new (entity.Id, entity.Year, entity.CurrentNumber);

        return domain;
    }

    #endregion

    #region Convert methods from domain model to entity

    /// <summary>
    /// Convert from domain model to entity
    /// </summary>
    public static EucharisticYouthRunningNumberEntity ToEntity(this EucharisticYouthRunningNumber domain)
    {
        EucharisticYouthRunningNumberEntity entity = new EucharisticYouthRunningNumberEntity
        {
            Id = domain.Id,
            Year = domain.Year,
            CurrentNumber = domain.CurrentNumber
        };

        return entity;
    }

    /// <summary>
    /// Update entity
    /// </summary>
    public static void UpdateEntity(this EucharisticYouthRunningNumber domain, EucharisticYouthRunningNumberEntity entity)
    {
        entity.CurrentNumber = domain.CurrentNumber;
    }

    #endregion
}