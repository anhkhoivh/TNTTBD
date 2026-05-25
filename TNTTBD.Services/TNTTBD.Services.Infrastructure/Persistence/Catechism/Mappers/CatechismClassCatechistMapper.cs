using TNTTBD.Services.Domain.Catechism.JoinModels;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

/// <summary>
/// User mapper
/// </summary>
public static class CatechismClassCatechistMapper
{
    #region Convert methods from entity to domain model

    /// <summary>
    /// Convert from entity to domain model
    /// </summary>
    public static CatechismClassCatechist ToDomain(this CatechismClassCatechistEntity entity)
    {
        CatechismClassCatechist domain = new CatechismClassCatechist(
            entity.Id, entity.CatechistId, entity.CatechismClassId, entity.Role,
            entity.EnrolledAt, entity.LeftAt, entity.Status, entity.Note);

        return domain;
    }

    #endregion

    #region Convert methods from domain model to entity

    /// <summary>
    /// Convert from domain model to entity
    /// </summary>
    public static CatechismClassCatechistEntity ToEntity(this CatechismClassCatechist domain)
    {
        CatechismClassCatechistEntity entity = new CatechismClassCatechistEntity
        {
            Id = domain.Id,
            CatechismClassId = domain.CatechismClassId,
            CatechistId = domain.CatechistId,
            Role = domain.Role,
            EnrolledAt = domain.EnrolledAt,
            LeftAt = domain.LeftAt,
            Status = domain.Status,
            Note = domain.Note
        };

        return entity;
    }

    /// <summary>
    /// Update entity
    /// </summary>
    public static void UpdateEntity(this CatechismClassCatechist domain, CatechismClassCatechistEntity entity)
    {
        entity.CatechismClassId = domain.CatechismClassId;
        entity.CatechistId = domain.CatechistId;
        entity.Role = domain.Role;
        entity.LeftAt = domain.LeftAt;
        entity.Status = domain.Status;
        entity.Note = domain.Note;
    }

    #endregion
}