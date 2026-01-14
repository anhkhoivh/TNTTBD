using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

/// <summary>
/// ExamEucharisticYouthEntity entity
/// </summary>
public class ExamEucharisticYouthEntity
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public ExamEucharisticYouthEntity()
    { }

    #endregion

    #region Properties

    /// <summary>
    /// Id
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Test id
    /// </summary>
    [ForeignKey(nameof(ExamId))]
    public Guid ExamId { get; set; }

    /// <summary>
    /// Exam
    /// </summary>
    public ExamEntity Exam { get; set; }

    /// <summary>
    /// EY id
    /// </summary>
    public Guid CatechismClassEucharisticYouthId { get; set; }

    /// <summary>
    /// Test
    /// </summary>
    public CatechismClassEucharisticYouthEntity CatechismClassEucharisticYouth { get; set; }

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
    public string Remark { get; set; }

    #endregion
}
