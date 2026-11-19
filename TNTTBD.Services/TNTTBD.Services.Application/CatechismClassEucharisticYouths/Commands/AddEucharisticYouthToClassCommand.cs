namespace TNTTBD.Services.Application.CatechismClassEucharisticYouths.Commands;

public record AddEucharisticYouthToClassCommand(
    Guid CatechismClassId,
    Guid EucharisticYouthId
);