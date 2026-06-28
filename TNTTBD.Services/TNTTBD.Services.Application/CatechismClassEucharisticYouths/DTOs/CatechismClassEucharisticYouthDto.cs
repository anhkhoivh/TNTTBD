using TNTTBD.Services.Domain.Enums;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Application.CatechismClassEucharisticYouths.DTOs;

public class CatechismClassEucharisticYouthDto
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Catechism Class Id
    /// </summary>
    public Guid CatechismClassId { get; set; }

    /// <summary>
    /// Class Name
    /// </summary>
    public string ClassName { get; set; } = null!;

    /// <summary>
    /// Class Name
    /// </summary>
    public string CatechismYearTheme { get; set; } = null!;

    /// <summary>
    /// Catechist Id
    /// </summary>
    public Guid EucharisticYouthId { get; set; }

    /// <summary>
    /// Code
    /// </summary>
    public string Code { get; set; } = null!;

    /// <summary>
    /// Full name
    /// </summary>
    public required FullName FullName { get; init; }

    /// <summary>
    /// Date in
    /// </summary>
    public DateTimeOffset EnrolledAt { get; set; }

    /// <summary>
    /// Date out
    /// </summary>
    public DateTimeOffset? LeftAt { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    public EnrollmentStatus Status { get; set; }

    /// <summary>
    /// Note
    /// </summary>
    public string? Note { get; set; }
}