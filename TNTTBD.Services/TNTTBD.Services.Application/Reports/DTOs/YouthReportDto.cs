using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Application.Reports.DTOs;

public class YouthReportDto
{
    public Guid YouthId { get; set; }
    public string? YouthCode { get; set; }
    public FullName? YouthFullName { get; set; }
    public Guid? CatechismYearId { get; set; }
    public Guid? SelectedEnrollmentId { get; set; }
    public List<YouthClassReportDto> Enrollments { get; set; } = [];
    public YouthAttendanceSummaryDto Attendance { get; set; } = new();
    public YouthExamSummaryDto Exams { get; set; } = new();
    public List<YouthAttendanceReportItemDto> AttendanceDetails { get; set; } = [];
    public List<YouthExamReportItemDto> ExamDetails { get; set; } = [];
}