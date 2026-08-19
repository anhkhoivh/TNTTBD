using System.ComponentModel.DataAnnotations;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

/// <summary>
/// ExamResultEntity
/// </summary>
public class ExamResultEntity
{
    #region Properties

    /// <summary>
    /// Id
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Test id
    /// </summary>
    public Guid ExamId { get; set; }

    /// <summary>
    /// Exam
    /// </summary>
    public ExamEntity Exam { get; set; } = null!;

    /// <summary>
    /// EY id
    /// </summary>
    public Guid CatechismClassEucharisticYouthId { get; set; }

    /// <summary>
    /// Youth
    /// </summary>
    public CatechismClassEucharisticYouthEntity CatechismClassEucharisticYouth { get; set; } = null!;

    /// <summary>
    /// Score
    /// </summary>
    public double? Score { get; set; }

    /// <summary>
    /// Remark
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// Marked by
    /// </summary>
    public Guid MarkedByCatechistId { get; set; }

    /// <summary>
    /// Catechist mark
    /// </summary>
    public CatechistEntity MarkedByCatechist { get; set; } = null!;

    /// <summary>
    /// Marked at
    /// </summary>
    public DateTimeOffset MarkedTime { get; set; }

    #endregion
}
