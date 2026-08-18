using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Application.Exams.DTOs;

public class ExamResultDto
{
    public Guid? Id { get; set; }
    public Guid CatechismClassEucharisticYouthId { get; set; }
    public string? YouthCode { get; set; }
    public FullName? YouthFullName { get; set; }
    public double? Score { get; set; }
    public bool IsDone { get; set; }
    public string? Remark { get; set; }
    public DateTimeOffset MarkedTime { get; set; }
}
