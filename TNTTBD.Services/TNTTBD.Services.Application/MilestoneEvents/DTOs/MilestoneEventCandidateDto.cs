using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Application.MilestoneEvents.DTOs;

public class MilestoneEventCandidateDto
{
    public Guid EucharisticYouthId { get; set; }
    public string YouthCode { get; set; } = string.Empty;
    public FullName FullName { get; set; } = null!;
    public DateOnly? DateOfBirth { get; set; }
    public string? ClassName { get; set; }
    public bool HasBaptism { get; set; }
    public DateOnly? BaptismDate { get; set; }
    public MilestoneEventCandidateMilestoneDto? ExistingMilestone { get; set; }
    public List<MilestonePrerequisiteWarningDto> Warnings { get; set; } = [];
}
