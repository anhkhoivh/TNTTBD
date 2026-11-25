namespace TNTTBD.Services.Application.EucharisticYouths.Commands;

public record UpdateEucharisticYouthMilestoneCommand(
    Guid EucharisticYouthId,
    Guid MilestoneId,
    DateOnly Date,
    string? DocumentCode,
    string? Place,
    string? Note,
    Guid? SponsorContactId = null,
    string? Celebrant = null
);