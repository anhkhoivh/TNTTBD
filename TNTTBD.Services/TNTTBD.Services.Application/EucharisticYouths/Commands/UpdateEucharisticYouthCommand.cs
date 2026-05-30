using MediatR;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.EucharisticYouths.Commands;

public record UpdateEucharisticYouthCommand(
    Guid EucharisticYouthId,
    string GivenName, 
    string? ChristName, 
    string? FamilyName,
    string? MiddleName,
    DateOnly DateOfBirth,
    string? Address,
    bool IsMale,
    List<YouthPatronSaintData>? PatronSaints = null,
    List<YouthMilestoneData>? Milestones = null
) : IRequest;