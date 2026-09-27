namespace TNTTBD.Services.Application.Reports.DTOs;

public class ClassExamReportItemDto
{
    public Guid ExamId { get; set; }
    public string? Title { get; set; }
    public DateOnly ExamDate { get; set; }
    public int TotalResults { get; set; }
    public int Scored { get; set; }
    public double? AverageScore { get; set; }
}