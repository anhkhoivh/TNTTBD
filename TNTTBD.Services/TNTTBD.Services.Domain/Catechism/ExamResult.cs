using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TNTTBD.Services.Domain.Members;

namespace TNTTBD.Services.Domain.Catechism;

/// <summary>
/// Student entity
/// </summary>
public class ExamResult
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public ExamResult()
    { }

    #endregion

    #region Properties

    /// <summary>
    /// Id
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Point
    /// </summary>
    public double Point { get; set; }

    /// <summary>
    /// Exam
    /// </summary>
    public Exam Exam { get; set; }

    /// <summary>
    /// Test
    /// </summary>
    public EucharisticYouth EucharisticYouth { get; set; }

    #endregion
}
