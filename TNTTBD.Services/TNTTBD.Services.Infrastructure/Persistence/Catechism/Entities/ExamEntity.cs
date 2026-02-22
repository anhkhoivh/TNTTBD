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
    /// Exam Date
    /// </summary>
    public DateOnly ExamDate { get; set; }

    /// <summary>
    /// Attendance
    /// </summary>
    public ICollection<ExamResultEntity> ExamResults { get; set; } = new List<ExamResultEntity>();

    #endregion
}
