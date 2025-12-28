using System.ComponentModel.DataAnnotations;

namespace TNTTBD.Services.Domain.Catechism;

/// <summary>
/// Student entity
/// </summary>
public class Exam
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public Exam()
    { }

    #endregion

    #region Properties

    /// <summary>
    /// Id
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Class
    /// </summary>
    public CatechismClass Class { get; set; }

    /// <summary>
    /// Exam Result list
    /// </summary>
    public List<ExamResult> ExamResults { get; set; } = new List<ExamResult>();

    #endregion
}
