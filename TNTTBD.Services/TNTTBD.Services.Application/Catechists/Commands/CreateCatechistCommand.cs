using MediatR;

namespace TNTTBD.Services.Application.Catechists.Commands;

public record CreateCatechistCommand(
    string GivenName, 
    string? ChristName, 
    string? FamilyName,
    string? MiddleName,
    DateOnly DateOfBirth,
    string? Address,
    bool IsMale,
    List<CatechistPatronSaintData>? PatronSaints = null
) : IRequest<Guid>;

public record CatechistPatronSaintData(string SaintName, int Day, int Month);