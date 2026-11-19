namespace TNTTBD.Services.Application.EucharisticYouths.Commands;

public record RemoveEucharisticYouthPatronSaintCommand(
    Guid EucharisticYouthId,
    string SaintName,
    int Day, 
    int Month
);