using MediatR;

namespace TNTTBD.Services.Application.EucharisticYouths.Commands;

public record RemoveYouthPhotoCommand(
    Guid EucharisticYouthId,
    Guid PhotoId
) : IRequest;