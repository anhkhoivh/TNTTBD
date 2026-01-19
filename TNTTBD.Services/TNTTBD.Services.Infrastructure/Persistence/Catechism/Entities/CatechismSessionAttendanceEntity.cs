using System.ComponentModel.DataAnnotations;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

/// <summary>
/// Attendance entity
/// </summary>
public class CatechismSessionAttendanceEntity 
{
    #region Constructors
    #endregion

    #region Properties

    /// <summary>
    /// Attendance Id
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Id of session
    /// </summary>
    public Guid CatechismSessionId { get; set; }

    /// <summary>
    /// Session
    /// </summary>
    public CatechismSessionEntity CatechismSession { get; set; } = null!;

    /// <summary>
    /// Status
    /// </summary>
    public AttendanceStatus AttendanceStatus{ get; set; }

    /// <summary>
    /// Id of CatechismClassEucharisticYouth
    /// </summary>
    public Guid CatechismClassEucharisticYouthId { get; set; }

    /// <summary>
    /// CatechismClassEucharisticYouth
    /// </summary>
    public CatechismClassEucharisticYouthEntity CatechismClassEucharisticYouth { get; set; } = null!;

    #endregion
}
