using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Application.MilestoneEvents.DTOs;

public class MilestoneEventParticipantDto
{
    public Guid MilestoneId { get; set; }
    public Guid EucharisticYouthId { get; set; }
    public string YouthCode { get; set; } = string.Empty;
    public FullName FullName { get; set; } = null!;
    public DateOnly? DateOfBirth { get; set; }
    public string? ClassName { get; set; }
    public DateOnly? Date { get; set; }
    public string? Place { get; set; }
    public string? Celebrant { get; set; }
    public string? DocumentCode { get; set; }
    public MilestoneSponsorDto? Sponsor { get; set; }
    public string? Note { get; set; }
    public DateOnly? BaptismDate { get; set; }
    public List<MilestonePrerequisiteWarningDto> Warnings { get; set; } = [];
}
