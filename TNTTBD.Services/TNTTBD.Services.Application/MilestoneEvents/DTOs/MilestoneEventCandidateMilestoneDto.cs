namespace TNTTBD.Services.Application.MilestoneEvents.DTOs;

public class MilestoneEventCandidateMilestoneDto
{
    public Guid MilestoneId { get; set; }
    public DateOnly? Date { get; set; }
    public string? Place { get; set; }
    public string? Celebrant { get; set; }
    public string? DocumentCode { get; set; }
    public MilestoneSponsorDto? Sponsor { get; set; }
    public string? Note { get; set; }
    public Guid? MilestoneEventId { get; set; }
    public string? MilestoneEventName { get; set; }
}
