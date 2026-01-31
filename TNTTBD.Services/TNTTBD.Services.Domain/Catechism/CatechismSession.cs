using TNTTBD.Services.Domain.Common;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Domain.Catechism;

/// <summary>
/// Session entity
/// </summary>
public class CatechismSession : AggregateRoot<Guid>
{
    #region Fields

    private readonly List<CatechismSessionAttendance> _attendances = new List<CatechismSessionAttendance>();

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public CatechismSession() : base(Guid.Empty)
    {
        Title = string.Empty;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Date
    /// </summary>
    public DateTimeOffset DateTime { get; }

    /// <summary>
    /// Theme
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Attendance
    /// </summary>
    public IReadOnlyCollection<CatechismSessionAttendance> Attendances => _attendances;

    #endregion

    #region Methods

    /// <summary>
    /// Mark attendance
    /// </summary>
    public void MarkAttendance(
        Guid catechismClassEucharisticYouthId,
        AttendanceStatus status,
        Guid markedBy)
    {
        if (_attendances.Any(x => x.CatechismClassEucharisticYouthId == catechismClassEucharisticYouthId))
            throw new DomainException("Attendance already recorded.");

        _attendances.Add(new CatechismSessionAttendance(
            Guid.NewGuid(),
            Id,
            catechismClassEucharisticYouthId,
            status,
            DateTimeOffset.Now,
            markedBy
        ));
    }

    #endregion
}
