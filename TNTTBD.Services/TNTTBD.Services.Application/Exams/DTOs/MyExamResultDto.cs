namespace TNTTBD.Services.Application.Exams.DTOs;

public class MyExamResultDto
{
    public Guid ExamId { get; set; }
    public required string ExamTitle { get; set; }
    public DateOnly ExamDate { get; set; }
    public double Score { get; set; }
    public bool IsDone { get; set; }
    public string? Remark { get; set; }
}
