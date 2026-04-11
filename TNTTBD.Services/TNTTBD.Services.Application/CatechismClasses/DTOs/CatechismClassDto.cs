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
}