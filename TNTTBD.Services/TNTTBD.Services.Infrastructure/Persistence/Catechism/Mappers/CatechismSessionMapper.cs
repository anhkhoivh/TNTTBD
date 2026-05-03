using TNTTBD.Services.Application.CatechismSessions.DTOs;
using TNTTBD.Services.Domain.Catechism;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

/// <summary>
/// User mapper
/// </summary>
public static class CatechismSessionMapper
{
    #region Convert methods from entity to domain model

    /// <summary>
    /// Convert from entity to domain model
    /// </summary>
    public static CatechismSession ToDomain(this CatechismSessionEntity entity)
    {
        List<CatechismSessionAttendance> attendances = entity.EucharisticYouthAttendances.Select(a => a.ToDomain()).ToList();
        CatechismSession domain = new CatechismSession(entity.Id, entity.CatechismClassId, entity.DateTime, entity.Title, attendances);
        return domain;
    }

    #endregion

    #region Convert methods from domain model to entity

    /// <summary>
    /// Convert from domain model to entity
    /// </summary>
    public static CatechismSessionEntity ToEntity(this CatechismSession domain)
    {
        CatechismSessionEntity entity = new CatechismSessionEntity
        {
            Id = domain.Id,
            CatechismClassId = domain.CatechismClassId,
            Title = domain.Title,
            EucharisticYouthAttendances = domain.Attendances.Select(a => a.ToEntity()).ToList()
        };

        return entity;
    }

    /// <summary>
    /// Update entity
    /// </summary>
    public static void UpdateEntity(this CatechismSession domain, CatechismSessionEntity entity)
    {
        entity.DateTime = domain.DateTime;
        entity.CatechismClassId = domain.CatechismClassId;
        entity.Title = domain.Title;
        entity.EucharisticYouthAttendances.Clear();

        foreach (CatechismSessionAttendance attendance in domain.Attendances)
        {
            entity.EucharisticYouthAttendances.Add(attendance.ToEntity());
        }
    }

    #endregion

    #region Convert methods from entity to DTO

    /// <summary>
    /// Convert from entity to DTO
    /// </summary>
    public static CatechismSessionDto ToDTO(this CatechismSessionEntity entity)
    {
        return new CatechismSessionDto
            {
                Id = entity.Id,
                Title = entity.Title,
                CatechismClassId = entity.CatechismClassId,
                DateTime = entity.DateTime
            };
    }

    #endregion
}