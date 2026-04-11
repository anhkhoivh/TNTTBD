using MediatR;

namespace TNTTBD.Services.Application.CatechismClasses.Commands;

public record CreateCatechismClassCommand(
    DateOnly StartDate,
    DateOnly EndDate,
    string? Theme
) : IRequest<Guid>;