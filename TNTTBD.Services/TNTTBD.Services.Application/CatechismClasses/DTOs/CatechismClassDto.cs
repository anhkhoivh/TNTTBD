namespace TNTTBD.Services.Application.CatechismClasses.DTOs;

public class CatechismClassDto
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Class Name
    /// </summary>
    public required string ClassName { get; set; }

    /// <summary>
    /// Class Name
    /// </summary>
    public required string ClassCode { get; set; }

    /// <summary>
    /// Class Year Id
    /// </summary>
    public Guid CatechismYearId { get; set; }

    /// <summary>
    /// Class Year Id
    /// </summary>
    public required string CatechismYearTitle { get; set; }
}