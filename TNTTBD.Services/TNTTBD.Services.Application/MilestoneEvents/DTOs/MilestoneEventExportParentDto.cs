using TNTTBD.Services.Domain.Enums;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Application.MilestoneEvents.DTOs;

public class MilestoneEventExportParentDto
{
    public ContactRelationType Relation { get; set; }
    public FullName FullName { get; set; } = null!;
}
