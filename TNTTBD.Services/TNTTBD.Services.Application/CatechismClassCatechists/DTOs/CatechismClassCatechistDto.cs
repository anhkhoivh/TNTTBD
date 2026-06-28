using TNTTBD.Services.Domain.Enums;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Application.CatechismClassCatechists.DTOs;

public class CatechismClassCatechistDto
{
    public Guid Id { get; set; }
    public Guid CatechismClassId { get; set; }
    public required string ClassName { get; set; }
    public required string CatechismYearTheme { get; set; }
    public Guid CatechistId { get; set; }
    public required FullName FullName { get; init; }
    public CatechismClassCatechistRole Role { get; set; }
    public DateTimeOffset EnrolledAt { get; set; }
    public DateTimeOffset? LeftAt { get; set; }
    public EnrollmentStatus Status { get; set; }
    public string? Note { get; set; }
}
