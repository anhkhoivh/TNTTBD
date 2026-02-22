using TNTTBD.Services.Domain.Catechism;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

/// <summary>
/// User mapper
/// </summary>
public static class ExamResultMapper
{
    #region Convert methods from entity to domain model

    /// <summary>
    /// Convert from entity to domain model
    /// </summary>
    public static ExamResult ToDomain(this ExamResultEntity entity)
    {
        ExamResult domain = new ExamResult(
            entity.Id,
            entity.ExamId,
            entity.CatechismClassEucharisticYouthId,
            entity.MarkedByCatechistId,
            entity.MarkedTime,
            entity.Score,
            entity.IsDone,
            entity.Remark);

        return domain;
    }

    #endregion

    #region Convert methods from domain model to entity

    /// <summary>
    /// Convert from domain model to entity
    /// </summary>
    public static ExamResultEntity ToEntity(this ExamResult domain)
    {
        ExamResultEntity entity = new ExamResultEntity
        {
            Id = domain.Id,
            CatechismClassEucharisticYouthId = domain.CatechismClassEucharisticYouthId,
            ExamId = domain.ExamId,
            MarkedByCatechistId = domain.MarkedByCatechistId,
            MarkedTime = domain.MarkedTime,
            Score = domain.Score,
            IsDone = domain.IsDone,
            Remark = domain.Remark
        };

        return entity;
    }

    /// <summary>
    /// Update entity
    /// </summary>
    public static void UpdateEntity(this ExamResult domain, ExamResultEntity entity)
    {
        entity.CatechismClassEucharisticYouthId = domain.CatechismClassEucharisticYouthId;
        entity.ExamId = domain.ExamId;
        entity.MarkedByCatechistId = domain.MarkedByCatechistId;
        entity.MarkedTime = domain.MarkedTime;
        entity.Score = domain.Score;
        entity.IsDone = domain.IsDone;
        entity.Remark = domain.Remark;
    }

    #endregion
}