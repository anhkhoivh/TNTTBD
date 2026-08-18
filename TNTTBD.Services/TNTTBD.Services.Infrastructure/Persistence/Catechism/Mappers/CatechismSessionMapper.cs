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
            DateTime = domain.DateTime,
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
    }

    /// <summary>
    /// Update Attendances
    /// </summary>
    public static void UpdateAttendances(this CatechismSession domain, CatechismSessionEntity entity)
    {
        foreach (CatechismSessionAttendance attendance in domain.Attendances)
        {
            CatechismSessionAttendanceEntity? existingEntity = entity.EucharisticYouthAttendances.FirstOrDefault(x => x.Id == attendance.Id);

            if (existingEntity == null)
            {
                entity.EucharisticYouthAttendances.Add(attendance.ToEntity());
            }
            else
            {
                attendance.UpdateEntity(existingEntity);
            }
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

    /// <summary>
    /// Update Attendance
    /// </summary>
    public static void UpdateAttendance(this CatechismSessionEntity entity, CatechismSessionAttendance attendance)
    {
        CatechismSessionAttendanceEntity? existingEntity = entity.EucharisticYouthAttendances.FirstOrDefault(x => x.Id == attendance.Id);

        if (existingEntity == null)
        {
            entity.EucharisticYouthAttendances.Add(attendance.ToEntity());
        }
        else
        {
            attendance.UpdateEntity(existingEntity);
        }
    }

    #endregion
}