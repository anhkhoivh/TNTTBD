using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Application.MilestoneEvents.DTOs;

public class MilestoneEventExportRowDto
{
    public string? DocumentCode { get; set; }
    public FullName FullName { get; set; } = null!;
    public DateOnly? DateOfBirth { get; set; }
    public DateOnly? BaptismDate { get; set; }
    public string? Address { get; set; }
    public List<MilestoneEventExportParentDto> Parents { get; set; } = [];
    public FullName? SponsorFullName { get; set; }
    public string? Celebrant { get; set; }
}
