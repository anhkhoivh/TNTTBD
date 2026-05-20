using MediatR;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.CatechismClassEucharisticYouths.Commands;

public record MarkCatechismClassEucharisticYouthsCompletedCommand(
    IEnumerable<Guid> Ids
) : IRequest;