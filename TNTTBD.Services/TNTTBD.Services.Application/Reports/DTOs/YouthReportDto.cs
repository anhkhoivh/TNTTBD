using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Application.Reports.DTOs;

public class YouthReportDto
{
    public Guid YouthId { get; set; }
    public string? YouthCode { get; set; }
    public FullName? YouthFullName { get; set; }
    public YouthAttendanceSummaryDto Attendance { get; set; } = new();
    public YouthExamSummaryDto Exams { get; set; } = new();
    public List<YouthClassReportDto> Classes { get; set; } = [];
    public PaginatedResult<YouthAttendanceReportItemDto> AttendanceDetails { get; set; } = new();
    public PaginatedResult<YouthExamReportItemDto> ExamDetails { get; set; } = new();
}