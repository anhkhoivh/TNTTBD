namespace TNTTBD.Services.Application.Reports.DTOs;

public class ClassReportDto
{
    public Guid ClassId { get; set; }
    public string? ClassCode { get; set; }
    public string? ClassName { get; set; }
    public Guid CatechismYearId { get; set; }
    public string? CatechismYearName { get; set; }
    public int SessionsHeld { get; set; }
    public List<ClassReportSessionDto> Sessions { get; set; } = [];
    public List<ClassReportExamDto> Exams { get; set; } = [];
    public List<ClassReportRowDto> Rows { get; set; } = [];
}