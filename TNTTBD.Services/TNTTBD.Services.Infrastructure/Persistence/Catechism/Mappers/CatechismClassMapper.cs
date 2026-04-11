using TNTTBD.Services.Application.CatechismClasses.DTOs;
using TNTTBD.Services.Domain.Catechism;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

/// <summary>
/// User mapper
/// </summary>
public static class CatechismClassMapper
{
    #region Convert methods from entity to domain model

    /// <summary>
    /// Convert from entity to domain model
    /// </summary>
    public static CatechismClass ToDomain(this CatechismClassEntity entity)
    {
        CatechismClass domain = new CatechismClass(entity.Id, entity.CatechismYearId, entity.ClassName);

        return domain;
    }

    #endregion

    #region Convert methods from domain model to entity

    /// <summary>
    /// Convert from domain model to entity
    /// </summary>
    public static CatechismClassEntity ToEntity(this CatechismClass domain)
    {
        CatechismClassEntity entity = new CatechismClassEntity
        {
            Id = domain.Id,
            CatechismYearId = domain.CatechismYearId,
            ClassName = domain.ClassName
        };

        return entity;
    }

    /// <summary>
    /// Update entity
    /// </summary>
    public static void UpdateEntity(this CatechismClass domain, CatechismClassEntity entity)
    {
        entity.ClassName = domain.ClassName;
    }

    #endregion

    #region Convert methods from entity to DTO

    /// <summary>
    /// Convert from entity to DTO
    /// </summary>
    public static CatechismClassDto ToDTO(this CatechismClassEntity entity)
    {
        return new CatechismClassDto
            {
                Id = entity.Id,
                ClassName = entity.ClassName
            };
    }

    #endregion
}