namespace TNTTBD.Services.Application.Reports.DTOs;

public class YouthExamReportItemDto
{
    public Guid ExamId { get; set; }
    public string? ExamTitle { get; set; }
    public DateOnly ExamDate { get; set; }
    public Guid ClassId { get; set; }
    public string? ClassName { get; set; }
    public double? Score { get; set; }
    public string? Remark { get; set; }
}