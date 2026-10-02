using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.Reports.DTOs;

public class ClassReportAttendanceCellDto
{
    public Guid SessionId { get; set; }
    public AttendanceStatus Status { get; set; }
    public bool IsCounted { get; set; }
}
