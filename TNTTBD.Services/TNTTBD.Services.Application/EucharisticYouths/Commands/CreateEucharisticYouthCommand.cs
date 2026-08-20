using MediatR;
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
    List<YouthPatronSaintData>? PatronSaints = null,
    List<YouthMilestoneData>? Milestones = null,
    List<YouthContactData>? Contacts = null,
    Guid? CatechismClassId = null
) : IRequest<Guid>;

public record YouthPatronSaintData(string SaintName, int Day, int Month);

public record YouthMilestoneData(
    MilestoneType Type,
    DateOnly? Date,
    string? Place,
    string? DocumentCode,
    string? Note
);

public record YouthContactData(
    ContactRelationType RelationType,
    string GivenName,
    string? ChristName,
    string? FamilyName,
    string? MiddleName,
    string? PhoneNumber,
    string? Email
);