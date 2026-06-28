using System.ComponentModel.DataAnnotations;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

public class CatechismClassEucharisticYouthEntity
{
    #region Properties

    /// <summary>
    /// Id
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Catechism Class Id
    /// </summary>
    public Guid CatechismClassId { get; set; }

    /// <summary>
    /// Catechism Class entity
    /// </summary>
    public CatechismClassEntity CatechismClass { get; set; } = null!;

    /// <summary>
    /// EucharisticYouth Id
    /// </summary>
    public Guid EucharisticYouthId { get; set; }

    /// <summary>
    /// Youth
    /// </summary>
    public EucharisticYouthEntity EucharisticYouth { get; set; } = null!;

    /// <summary>
    /// Date in
    /// </summary>
    public DateTimeOffset EnrolledAt { get; set; }

    /// <summary>
    /// Date out
    /// </summary>
    public DateTimeOffset? LeftAt { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    public EnrollmentStatus Status { get; set; }

    /// <summary>
    /// Note
    /// </summary>
    public string? Note { get; set; }

    /// <summary>
    /// Attendance
    /// </summary>
    public ICollection<CatechismSessionAttendanceEntity> EucharisticYouthAttendances { get; set; } = new List<CatechismSessionAttendanceEntity>();

    /// <summary>
    /// Results
    /// </summary>
    public ICollection<ExamResultEntity> ExamResults { get; set; } = new List<ExamResultEntity>();

    #endregion
}