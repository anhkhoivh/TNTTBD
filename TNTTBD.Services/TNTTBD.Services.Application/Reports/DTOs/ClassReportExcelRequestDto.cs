namespace TNTTBD.Services.Application.Reports.DTOs;

public class ClassReportExcelRequestDto
{
    public List<Guid> AttendanceEnrollmentIds { get; set; } = [];
    public List<Guid> ExamEnrollmentIds { get; set; } = [];
    public string Language { get; set; } = "vi";
}