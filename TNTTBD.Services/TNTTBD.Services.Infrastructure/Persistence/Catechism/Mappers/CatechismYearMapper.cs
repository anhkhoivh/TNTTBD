using TNTTBD.Services.Application.CatechismYears.DTOs;
using TNTTBD.Services.Domain.Catechism;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

/// <summary>
/// User mapper
/// </summary>
public static class CatechismYearMapper
{
    #region Convert methods from entity to domain model

    /// <summary>
    /// Convert from entity to domain model
    /// </summary>
    public static CatechismYear ToDomain(this CatechismYearEntity entity)
    {
        CatechismYear domain = new CatechismYear(entity.Id, entity.StartDate, entity.EndDate, entity.Theme);

        return domain;
    }

    #endregion

    #region Convert methods from domain model to entity

    /// <summary>
    /// Convert from domain model to entity
    /// </summary>
    public static CatechismYearEntity ToEntity(this CatechismYear domain)
    {
        CatechismYearEntity entity = new CatechismYearEntity
        {
            Id = domain.Id,
            StartDate = domain.StartDate,
            EndDate = domain.EndDate,
            Theme = domain.Theme
        };

        return entity;
    }

    /// <summary>
    /// Update entity
    /// </summary>
    public static void UpdateEntity(this CatechismYear domain, CatechismYearEntity entity)
    {
        entity.StartDate = domain.StartDate;
        entity.EndDate = domain.EndDate;
        entity.Theme = domain.Theme;
    }

    #endregion

    #region Convert methods from entity to DTO

    /// <summary>
    /// Convert from entity to DTO
    /// </summary>
    public static CatechismYearDto ToDTO(this CatechismYearEntity entity)
    {
        return new CatechismYearDto
            {
                Id = entity.Id,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Theme = entity.Theme,
                CatechismClasses = entity.CatechismClasses.Select(c => c.ToDTO()).ToList()
            };
    }

    #endregion
}