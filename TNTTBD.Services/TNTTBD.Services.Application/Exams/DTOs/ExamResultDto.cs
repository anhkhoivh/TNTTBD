namespace TNTTBD.Services.Application.Exams.DTOs;

public class ExamResultDto
{
    public Guid Id { get; set; }
    public Guid ExamId { get; set; }
    public Guid CatechismClassEucharisticYouthId { get; set; }
    public double Score { get; set; }
    public bool IsDone { get; set; }
    public string? Remark { get; set; }
    public Guid MarkedByCatechistId { get; set; }
    public DateTimeOffset MarkedTime { get; set; }
}
