using MediatR;

namespace TNTTBD.Services.Application.CatechismClassEucharisticYouths.Commands;

public record RemoveEucharisticYouthFromClassCommand(
    Guid Id
) : IRequest<bool>;