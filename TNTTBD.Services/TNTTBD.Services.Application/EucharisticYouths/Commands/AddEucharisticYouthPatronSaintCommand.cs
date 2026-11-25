namespace TNTTBD.Services.Application.EucharisticYouths.Commands;

public record AddEucharisticYouthPatronSaintCommand(
    Guid EucharisticYouthId,
    string SaintName,
    int Day,
    int Month
);