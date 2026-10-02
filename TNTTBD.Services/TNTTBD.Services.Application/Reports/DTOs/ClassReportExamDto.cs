namespace TNTTBD.Services.Application.Reports.DTOs;

public class ClassReportExamDto
{
    public Guid ExamId { get; set; }
    public string? Title { get; set; }
    public DateOnly ExamDate { get; set; }
}
