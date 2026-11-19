namespace TNTTBD.Services.Application.EucharisticYouths.Commands;

public record RemoveEucharisticYouthMilestoneCommand(
    Guid EucharisticYouthId,
    Guid MilestoneId
);