namespace TNTTBD.Services.Application.CatechismClassEucharisticYouths.Commands;

public record MigrateYouthToClassCommand(
    Guid SourceClassId,
    Guid DestinationClassId,
    List<Guid> EucharisticYouthIds
);
