using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.Enums;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Application.Common.DTOs;

/// <summary>
/// Spiritual Milestone Dto. <see cref="MilestoneEventId"/> / <see cref="MilestoneEventName"/> and the sponsor name/phone
/// are read-only and are ignored on write (event links are managed from the milestone event page).
/// </summary>
public record SpiritualMilestoneDto(
    Guid Id,
    MilestoneType Type,
    DateOnly? Date,
    string? Place,
    string? DocumentCode,
    string? Note,
    Guid? SponsorContactId = null,
    string? Celebrant = null,
    Guid? MilestoneEventId = null,
    string? MilestoneEventName = null,
    FullName? SponsorFullName = null,
    string? SponsorPhoneNumber = null
)
{
    public SpiritualMilestone ToDomain()
    {
        return new SpiritualMilestone(
            Id,
            Type,
            Date,
            DocumentCode,
            Place,
            Note,
            SponsorContactId,
            Celebrant
        );
    }
}
