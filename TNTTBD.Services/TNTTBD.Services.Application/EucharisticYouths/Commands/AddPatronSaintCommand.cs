using MediatR;

namespace TNTTBD.Services.Application.EucharisticYouths.Commands;

public record AddPatronSaintCommand(
    Guid EucharisticYouthId,
    string SaintName,
    int Day,
    int Month
) : IRequest;