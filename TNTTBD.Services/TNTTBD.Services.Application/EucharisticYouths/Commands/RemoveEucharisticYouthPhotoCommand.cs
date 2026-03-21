using MediatR;

namespace TNTTBD.Services.Application.EucharisticYouths.Commands;

public record RemoveEucharisticYouthPhotoCommand(
    Guid EucharisticYouthId,
    Guid PhotoId
) : IRequest;