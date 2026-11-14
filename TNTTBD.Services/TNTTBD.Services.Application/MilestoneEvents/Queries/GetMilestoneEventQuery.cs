using MediatR;
using TNTTBD.Services.Application.MilestoneEvents.DTOs;

namespace TNTTBD.Services.Application.MilestoneEvents.Queries;

public record GetMilestoneEventQuery(Guid Id) : IRequest<MilestoneEventDetailDto?>;
