using MediatR;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.EucharisticYouths.Commands;

public record AddEucharisticYouthMilestoneCommand(
    Guid EucharisticYouthId,
    MilestoneType Type,
    DateOnly Date,
    string? DocumentCode,
    string? Place,
    string? Note
) : IRequest<bool>;