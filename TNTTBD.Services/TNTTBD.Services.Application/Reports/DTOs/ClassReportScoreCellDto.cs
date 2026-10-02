namespace TNTTBD.Services.Application.Reports.DTOs;

public class ClassReportScoreCellDto
{
    public Guid ExamId { get; set; }
    public double? Score { get; set; }
    public string? Remark { get; set; }
}
