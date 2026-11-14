using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.MilestoneEvents.DTOs;

public class MilestoneEventDetailDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public MilestoneType Type { get; set; }
    public DateOnly Date { get; set; }
    public string? Place { get; set; }
    public string? Celebrant { get; set; }
    public string? Note { get; set; }
    public string? CodeTemplate { get; set; }
    public int StartIndex { get; set; }
    public List<MilestoneEventParticipantDto> Participants { get; set; } = [];
}
