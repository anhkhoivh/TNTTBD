using MediatR;

namespace TNTTBD.Services.Application.MilestoneEvents.Commands;

public record DeleteMilestoneEventCommand(Guid Id) : IRequest<bool>;
