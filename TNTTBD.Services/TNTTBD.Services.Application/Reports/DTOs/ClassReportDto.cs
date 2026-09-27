namespace TNTTBD.Services.Application.Reports.DTOs;

public class ClassReportDto
{
    public Guid ClassId { get; set; }
    public string? ClassCode { get; set; }
    public string? ClassName { get; set; }
    public Guid CatechismYearId { get; set; }
    public string? CatechismYearName { get; set; }
    public List<ClassSessionReportItemDto> Sessions { get; set; } = [];
    public List<ClassExamReportItemDto> Exams { get; set; } = [];
}