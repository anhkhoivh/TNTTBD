using MediatR;
using TNTTBD.Services.Application.MilestoneEvents.DTOs;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.MilestoneEvents.Commands;

/// <summary>
/// Create (Id null) or update a milestone event together with its participant list.
/// </summary>
public record SaveMilestoneEventCommand(
    Guid? Id,
    string Name,
    MilestoneType Type,
    DateOnly Date,
    string? Place,
    string? Celebrant,
    string? Note,
    string? CodeTemplate,
    int StartIndex,
    List<SaveMilestoneEventParticipantDto> Participants
) : IRequest<Guid>;
