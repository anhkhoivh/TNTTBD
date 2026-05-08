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

    /// <summary>
    /// Constructor from details
    /// </summary>
    public CatechismSession(Guid id, Guid classId, DateTimeOffset dateTime, string title, IEnumerable<CatechismSessionAttendance>? attendances = null) : base(id)
    {
        CatechismClassId = classId;
        DateTime = dateTime;
        Title = title;
        _attendances = attendances?.ToList() ?? new List<CatechismSessionAttendance>();
    }

    #endregion

    #region Properties

    /// <summary>
    /// Class Id
    /// </summary>
    public Guid CatechismClassId { get; private set; }

    /// <summary>
    /// Date
    /// </summary>
    public DateTimeOffset DateTime { get; private set; }

    /// <summary>
    /// Theme
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// Attendance
    /// </summary>
    public IReadOnlyCollection<CatechismSessionAttendance> Attendances => _attendances;

    #endregion

    #region Methods

    /// <summary>
    /// Update info
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="title"></param>
    public void UpdateInfo(DateTimeOffset dateTime, string title)
    {
        DateTime = dateTime;
        Title = title;
    }

    /// <summary>
    /// Mark attendance
    /// </summary>
    public CatechismSessionAttendance MarkAttendance(Guid catechismClassEucharisticYouthId, AttendanceStatus status, Guid markedBy)
    {
        CatechismSessionAttendance? existingAttendance = _attendances.FirstOrDefault(x => x.CatechismClassEucharisticYouthId == catechismClassEucharisticYouthId);

        if (existingAttendance == null)
        {
            CatechismSessionAttendance newAttendance = new(Guid.NewGuid(), Id, catechismClassEucharisticYouthId, status, DateTimeOffset.Now, markedBy);
            _attendances.Add(newAttendance);
            return newAttendance;
        }
        else
        {
            existingAttendance.Update(status, DateTimeOffset.Now, markedBy);
            return existingAttendance;
        }
    }

    /// <summary>
    /// For multiple attendances
    /// </summary>
    /// <param name="attendances"></param>
    /// <param name="catechistId"></param>
    public void MarkAttendances(IEnumerable<(Guid CatechismClassEucharisticYouthId, AttendanceStatus Status)> attendances, Guid catechistId)
    {
        foreach (var (youthId, status) in attendances)
        {
            MarkAttendance(youthId, status, catechistId);
        }
    }

    #endregion
}
