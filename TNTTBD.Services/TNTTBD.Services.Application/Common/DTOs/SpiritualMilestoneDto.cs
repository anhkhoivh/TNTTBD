using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.Common.DTOs;

/// <summary>
/// Spiritual Milestone Dto
/// </summary>
public record SpiritualMilestoneDto(
    Guid Id, 
    MilestoneType Type,
    DateOnly? Date,
    string? Place,
    string? DocumentCode,
    string? Note
)
{
    public SpiritualMilestone ToDomain()
    {
        return new SpiritualMilestone(
            Id,
            Type,
            Date,
            Place,
            DocumentCode,
            Note
        );
    }
}