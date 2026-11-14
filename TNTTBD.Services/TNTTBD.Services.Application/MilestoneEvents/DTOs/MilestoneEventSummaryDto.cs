using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.MilestoneEvents.DTOs;

public class MilestoneEventSummaryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public MilestoneType Type { get; set; }
    public DateOnly Date { get; set; }
    public string? Place { get; set; }
    public int ParticipantCount { get; set; }
}
