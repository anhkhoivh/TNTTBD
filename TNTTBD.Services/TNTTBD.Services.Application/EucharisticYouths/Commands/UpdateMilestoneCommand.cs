using MediatR;

namespace TNTTBD.Services.Application.EucharisticYouths.Commands;

public record UpdateMilestoneCommand(
    Guid EucharisticYouthId,
    Guid MilestoneId,
    DateOnly Date,
    string? DocumentCode,
    string? Place,
    string? Note
) : IRequest;