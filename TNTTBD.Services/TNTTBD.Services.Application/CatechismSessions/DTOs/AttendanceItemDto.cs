using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.CatechismSessions.DTOs;

public class AttendanceItemDto
{
    /// <summary>
    /// Status
    /// </summary>
    public AttendanceStatus AttendanceStatus{ get; set; }

    /// <summary>
    /// Id of EucharisticYouth
    /// </summary>
    public Guid CatechismClassEucharisticYouthId { get; set; }
}