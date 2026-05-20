using TNTTBD.Services.Domain.Enums;

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
    /// Christ Name
    /// </summary>
    public string? ChristName { get; set; }

    /// <summary>
    /// Family Name
    /// </summary>
    public string? FamilyName { get; set; }

    /// <summary>
    /// Middle Name
    /// </summary>
    public string? MiddleName { get; set; }

    /// <summary>
    /// Given Name
    /// </summary>
    public string GivenName { get; set; } = null!;

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