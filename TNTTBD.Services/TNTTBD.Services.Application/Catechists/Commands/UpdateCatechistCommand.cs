using MediatR;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Application.Catechists.Commands;

public record UpdateCatechistCommand(
    Guid CatechistId,
    string GivenName, 
    string? ChristName, 
    string? FamilyName,
    string? MiddleName,
    DateOnly DateOfBirth,
    string? Address,
    bool IsMale,
    IReadOnlyCollection<PatronSaintDto>? PatronSaints = null,
    string? PhoneNumber = null
) : IRequest<bool>;