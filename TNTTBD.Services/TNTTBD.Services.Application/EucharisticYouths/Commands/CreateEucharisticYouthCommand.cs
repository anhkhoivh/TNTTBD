using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.EucharisticYouths.Commands;

public record CreateEucharisticYouthCommand(
    string GivenName, 
    string? ChristName, 
    string? FamilyName,
    string? MiddleName,
    DateOnly DateOfBirth,
    string? Address,
    bool IsMale,
    List<PatronSaintDto>? PatronSaints = null,
    List<SpiritualMilestoneDto>? Milestones = null,
    List<YouthContactData>? Contacts = null,
    Guid? CatechismClassId = null
);

/// <summary>
/// Contact to link on youth creation. When <see cref="ContactId"/> is set, the existing contact is linked
/// and the name/phone/email fields are ignored; otherwise a new contact is created from them.
/// </summary>
public record YouthContactData(
    ContactRelationType RelationType,
    string? GivenName = null,
    string? ChristName = null,
    string? FamilyName = null,
    string? MiddleName = null,
    string? PhoneNumber = null,
    string? Email = null,
    Guid? ContactId = null
);