using TNTTBD.Services.Domain.Catechism;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.MilestoneEvents.DTOs;

public class MilestonePrerequisiteWarningDto
{
    public MilestoneType Type { get; set; }
    public MilestonePrerequisiteIssue Issue { get; set; }
}
