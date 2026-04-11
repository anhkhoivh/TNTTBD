using MediatR;

namespace TNTTBD.Services.Application.CatechismClasses.Commands;

public record UpdateCatechismClassCommand(
    Guid CatechismClassId,
    DateOnly StartDate,
    DateOnly EndDate,
    string? Theme
) : IRequest;