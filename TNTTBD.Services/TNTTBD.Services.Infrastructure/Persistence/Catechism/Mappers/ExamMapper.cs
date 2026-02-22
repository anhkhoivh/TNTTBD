using TNTTBD.Services.Domain.Catechism;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

/// <summary>
/// User mapper
/// </summary>
public static class ExamMapper
{
    #region Convert methods from entity to domain model

    /// <summary>
    /// Convert from entity to domain model
    /// </summary>
    public static Exam ToDomain(this ExamEntity entity)
    {
        IEnumerable<ExamResult> results = entity.ExamResults.Select(e => e.ToDomain());
        Exam domain = new Exam(entity.Id, entity.CatechismClassId, entity.ExamDate, entity.Title, results);

        return domain;
    }

    #endregion

    #region Convert methods from domain model to entity

    /// <summary>
    /// Convert from domain model to entity
    /// </summary>
    public static ExamEntity ToEntity(this Exam domain)
    {
        ExamEntity entity = new ExamEntity
        {
            Id = domain.Id,
            CatechismClassId = domain.CatechismClassId,
            ExamDate = domain.ExamDate,
            Title = domain.Title
        };

        return entity;
    }

    /// <summary>
    /// Update entity
    /// </summary>
    public static void UpdateEntity(this Exam domain, ExamEntity entity)
    {
        entity.CatechismClassId = domain.CatechismClassId;
        entity.ExamDate = domain.ExamDate;
        entity.Title = domain.Title;
    }

    #endregion
}