using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.Reports.DTOs;

public class YouthClassReportDto
{
    public Guid ClassId { get; set; }
    public string? ClassCode { get; set; }
    public string? ClassName { get; set; }
    public Guid CatechismYearId { get; set; }
    public string? CatechismYearName { get; set; }
    public EnrollmentStatus EnrollmentStatus { get; set; }
    public DateTimeOffset EnrolledAt { get; set; }
    public DateTimeOffset? LeftAt { get; set; }
}