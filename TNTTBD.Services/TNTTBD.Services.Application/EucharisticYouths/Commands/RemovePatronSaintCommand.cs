using MediatR;

namespace TNTTBD.Services.Application.EucharisticYouths.Commands;

public record RemovePatronSaintCommand(
    Guid EucharisticYouthId,
    string SaintName,
    int Day, 
    int Month
) : IRequest;