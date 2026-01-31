using TNTTBD.Services.Domain.Common;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Domain.Catechism;

/// <summary>
/// Attendance entity
/// </summary>
public class CatechismSessionAttendance : DomainEntity<Guid>
{
    #region Constructors

    /// <summary>
    /// Constructor from details
    /// </summary>
    /// <param name="id"></param>
    /// <param name="catechismSessionId"></param>
    /// <param name="catechismClassEucharisticYouthId"></param>
    /// <param name="status"></param>
    /// <param name="markedTime"></param>
    /// <param name="markedBy"></param>
    public CatechismSessionAttendance(
        Guid id,
        Guid catechismSessionId,
        Guid catechismClassEucharisticYouthId,
        AttendanceStatus status,
        DateTimeOffset markedTime,
        Guid? markedBy) : base(id)
    {
        CatechismClassEucharisticYouthId = catechismClassEucharisticYouthId;
        CatechismSessionId = catechismSessionId;
        AttendanceStatus = status;
        MarkedTime = markedTime;
        MarkedBy = markedBy;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Status
    /// </summary>
    public AttendanceStatus AttendanceStatus{ get; private set; }

    /// <summary>
    /// CatechismClassEucharisticYouth
    /// </summary>
    public Guid CatechismClassEucharisticYouthId { get; }

    /// <summary>
    /// Catechism Session Id
    /// </summary>
    public Guid CatechismSessionId { get; }

    /// <summary>
    /// Marked at
    /// </summary>
    public DateTimeOffset MarkedTime { get; private set; }

    /// <summary>
    /// Marked by
    /// </summary>
    public Guid? MarkedBy { get; private set; }
    
    #endregion

    #region Methods

    /// <summary>
    /// Update
    /// </summary>
    public void Update(
        AttendanceStatus status,
        DateTimeOffset markedAt,
        Guid? markedBy)
    {
        AttendanceStatus = status;
        MarkedTime = markedAt;
        MarkedBy = markedBy;
    }

    #endregion
}
