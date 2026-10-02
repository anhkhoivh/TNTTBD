using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.Reports.DTOs;

public class YouthAttendanceReportItemDto
{
    public Guid SessionId { get; set; }
    public string? SessionTitle { get; set; }
    public DateTimeOffset SessionDateTime { get; set; }
    public Guid ClassId { get; set; }
    public string? ClassName { get; set; }
    public AttendanceStatus Status { get; set; }
    public bool IsCounted { get; set; }
}