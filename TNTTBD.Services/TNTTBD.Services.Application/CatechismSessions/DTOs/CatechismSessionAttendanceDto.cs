using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.CatechismSessions.DTOs;

public class CatechismSessionAttendanceDto
{
    public Guid Id { get; set; }
    public Guid CatechismSessionId { get; set; }
    public Guid CatechismClassEucharisticYouthId { get; set; }
    public AttendanceStatus AttendanceStatus { get; set; }
    public DateTimeOffset MarkedTime { get; set; }
    public Guid MarkedByCatechistId { get; set; }
}
