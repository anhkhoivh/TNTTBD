using TNTTBD.Services.Application.CatechismClasses.DTOs;

namespace TNTTBD.Services.Application.CatechismYears.DTOs;

public class CatechismYearDto
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Start Date
    /// </summary>
    public DateOnly StartDate { get; set; }

    /// <summary>
    /// End Date
    /// </summary>
    public DateOnly EndDate { get; set; }

    /// <summary>
    /// Theme
    /// </summary>
    public string? Theme { get; set; }

    /// <summary>
    /// List of classes
    /// </summary>
    public List<CatechismClassDto> CatechismClasses { get; set; } = new();
}