using System.ComponentModel.DataAnnotations;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

/// <summary>
/// Session entity
/// </summary>
public class CatechismSessionEntity
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public CatechismSessionEntity()
    { }

    #endregion

    #region Properties

    /// <summary>
    /// Id
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Date
    /// </summary>
    public DateTimeOffset DateTime { get; set; }

    /// <summary>
    /// Title
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Class Id
    /// </summary>
    public Guid CatechismClassId { get; set; }

    /// <summary>
    /// Attendance
    /// </summary>
    public ICollection<CatechismSessionAttendanceEntity> EucharisticYouthAttendances { get; set; } = new List<CatechismSessionAttendanceEntity>();

    #endregion

}
