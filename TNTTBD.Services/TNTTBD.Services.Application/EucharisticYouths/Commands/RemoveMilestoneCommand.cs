using MediatR;

namespace TNTTBD.Services.Application.EucharisticYouths.Commands;

public record RemoveMilestoneCommand(
    Guid EucharisticYouthId,
    Guid MilestoneId
) : IRequest;