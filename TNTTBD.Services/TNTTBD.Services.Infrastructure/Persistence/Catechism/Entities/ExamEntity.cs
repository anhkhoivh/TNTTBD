using System.ComponentModel.DataAnnotations;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

/// <summary>
/// Student entity
/// </summary>
public class ExamEntity
{
    #region Properties

    /// <summary>
    /// Id
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Title
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Class Id
    /// </summary>
    public Guid CatechismClassId { get; set; }

    /// <summary>
    /// Class
    /// </summary>
    public CatechismClassEntity CatechismClass { get; set; } = null!;

    /// <summary>
    /// Exam Date
    /// </summary>
    public DateOnly ExamDate { get; set; }

    /// <summary>
    /// Results
    /// </summary>
    public ICollection<ExamResultEntity> ExamResults { get; set; } = new List<ExamResultEntity>();

    #endregion
}
