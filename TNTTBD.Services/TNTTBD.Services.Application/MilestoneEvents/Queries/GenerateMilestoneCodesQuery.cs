using MediatR;
using TNTTBD.Services.Application.MilestoneEvents.DTOs;

namespace TNTTBD.Services.Application.MilestoneEvents.Queries;

public record GenerateMilestoneCodesQuery(
    string? Template,
    int StartIndex,
    List<DateOnly> Dates
) : IRequest<GenerateMilestoneCodesResultDto>;
