namespace TNTTBD.Services.Application.Exams.DTOs;

public class ExamDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public Guid CatechismClassId { get; set; }
    public DateOnly ExamDate { get; set; }
    public List<ExamResultDto> ExamResults { get; set; } = [];
}
