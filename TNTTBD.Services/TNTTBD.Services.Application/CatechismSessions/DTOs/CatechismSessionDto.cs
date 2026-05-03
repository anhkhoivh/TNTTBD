namespace TNTTBD.Services.Application.CatechismSessions.DTOs;

public class CatechismSessionDto
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Date
    /// </summary>
    public DateTimeOffset DateTime { get; set; }

    /// <summary>
    /// Title
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// Class Id
    /// </summary>
    public Guid CatechismClassId { get; set; }
}