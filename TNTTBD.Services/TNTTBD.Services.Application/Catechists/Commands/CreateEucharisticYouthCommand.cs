using MediatR;

namespace TNTTBD.Services.Application.Catechists.Commands;

public record CreateCatechistCommand(
    string GivenName, 
    string? ChristName, 
    string? FamilyName,
    string? MiddleName,
    DateOnly DateOfBirth,
    string? Address,
    bool IsMale
) : IRequest<Guid>;