namespace TNTTBD.Services.Application.Common.DTOs;

/// <summary>
/// Spiritual Milestone Dto
/// </summary>
public class SpiritualMilestoneDto
{
    public required string Type { get; set; }
    public DateOnly Date { get; set; }
    public string? Place { get; set; }
    public string? Note { get; set; }
}