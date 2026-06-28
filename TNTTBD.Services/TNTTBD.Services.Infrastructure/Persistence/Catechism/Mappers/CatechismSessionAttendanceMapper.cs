using TNTTBD.Services.Application.CatechismSessions.DTOs;
using TNTTBD.Services.Domain.Catechism;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

/// <summary>
/// User mapper
/// </summary>
public static class CatechismSessionAttendanceMapper
{
    #region Convert methods from entity to domain model

    /// <summary>
    /// Convert from entity to domain model
    /// </summary>
    public static CatechismSessionAttendance ToDomain(this CatechismSessionAttendanceEntity entity)
    {
        CatechismSessionAttendance domain = new CatechismSessionAttendance(entity.Id, entity.CatechismSessionId, entity.CatechismClassEucharisticYouthId, entity.AttendanceStatus, entity.MarkedTime, entity.MarkedByCatechistId);
        return domain;
    }

    #endregion

    #region Convert methods from domain model to entity

    /// <summary>
    /// Convert from domain model to entity
    /// </summary>
    public static CatechismSessionAttendanceEntity ToEntity(this CatechismSessionAttendance domain)
    {
        CatechismSessionAttendanceEntity entity = new CatechismSessionAttendanceEntity
        {
            Id = domain.Id,
            CatechismSessionId = domain.CatechismSessionId,
            CatechismClassEucharisticYouthId = domain.CatechismClassEucharisticYouthId,
            AttendanceStatus = domain.AttendanceStatus,
            MarkedTime = domain.MarkedTime,
            MarkedByCatechistId = domain.MarkedByCatechistId
        };

        return entity;
    }

    /// <summary>
    /// Update entity
    /// </summary>
    public static void UpdateEntity(this CatechismSessionAttendance domain, CatechismSessionAttendanceEntity entity)
    {
        entity.CatechismSessionId = domain.CatechismSessionId;
        entity.CatechismClassEucharisticYouthId = domain.CatechismClassEucharisticYouthId;
        entity.AttendanceStatus = domain.AttendanceStatus;
        entity.MarkedTime = domain.MarkedTime;
        entity.MarkedByCatechistId = domain.MarkedByCatechistId;
    }

    /// <summary>
    /// Convert from entity to DTO
    /// </summary>
    public static CatechismSessionAttendanceDto ToDTO(this CatechismSessionAttendanceEntity entity)
    {
        return new CatechismSessionAttendanceDto
            {
                Id = entity.Id,
                CatechismSessionId = entity.CatechismSessionId,
                CatechismClassEucharisticYouthId = entity.CatechismClassEucharisticYouthId,
                AttendanceStatus = entity.AttendanceStatus,
                MarkedTime = entity.MarkedTime,
                MarkedByCatechistId = entity.MarkedByCatechistId
            };
    }

    #endregion
}