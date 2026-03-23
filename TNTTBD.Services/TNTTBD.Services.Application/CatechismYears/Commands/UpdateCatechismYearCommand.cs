using MediatR;

namespace TNTTBD.Services.Application.CatechismYears.Commands;

public record UpdateCatechismYearCommand(
    Guid CatechismYearId,
    DateOnly StartDate,
    DateOnly EndDate,
    string? Theme
) : IRequest;