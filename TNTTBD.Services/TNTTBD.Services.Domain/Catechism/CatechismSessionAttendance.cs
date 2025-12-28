using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Domain.Catechism;

/// <summary>
/// Attendance entity
/// </summary>
public class CatechismSessionAttendance
{
    #region Constructors
    #endregion

    #region Properties

    /// <summary>
    /// Attendance Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Session
    /// </summary>
    public CatechismSession CatechismSession { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    public AttendanceStatus AttendanceStatus{ get; set; }

    #endregion
}
