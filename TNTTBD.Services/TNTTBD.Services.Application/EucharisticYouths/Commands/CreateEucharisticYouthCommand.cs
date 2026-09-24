using MediatR;
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
) : IRequest<Guid?>;

public record YouthContactData(
    ContactRelationType RelationType,
    string GivenName,
    string? ChristName,
    string? FamilyName,
    string? MiddleName,
    string? PhoneNumber,
    string? Email
);