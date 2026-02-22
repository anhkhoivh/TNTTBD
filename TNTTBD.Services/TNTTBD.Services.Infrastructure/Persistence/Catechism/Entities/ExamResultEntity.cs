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
    /// EY id
    /// </summary>
    public Guid CatechismClassEucharisticYouthId { get; set; }

    /// <summary>
    /// Score
    /// </summary>
    public double Score { get; set; }

    /// <summary>
    /// Is absent
    /// </summary>
    public bool IsDone { get; set; }

    /// <summary>
    /// Remark
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// Marked by
    /// </summary>
    public Guid MarkedByCatechistId { get; set; }

    /// <summary>
    /// Marked at
    /// </summary>
    public DateTimeOffset MarkedTime { get; set; }

    #endregion
}
