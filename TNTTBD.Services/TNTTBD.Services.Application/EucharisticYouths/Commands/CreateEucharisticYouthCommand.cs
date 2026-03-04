using MediatR;

namespace TNTTBD.Services.Application.EucharisticYouths.Commands;

public record CreateEucharisticYouthCommand(
    string GivenName, 
    string? ChristName, 
    string? FamilyName,
    string? MiddleName,
    DateOnly DateOfBirth,
    string? Address,
    bool IsMale
) : IRequest<Guid>;