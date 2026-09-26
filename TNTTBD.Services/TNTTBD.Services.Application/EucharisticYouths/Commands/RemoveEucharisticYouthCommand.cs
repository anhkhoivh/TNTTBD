using MediatR;

namespace TNTTBD.Services.Application.EucharisticYouths.Commands;

public record RemoveEucharisticYouthCommand(
    Guid EucharisticYouthId
) : IRequest<bool>;