using MediatR;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Common.Exceptions;
using TNTTBD.Services.Application.MilestoneEvents.DTOs;
using TNTTBD.Services.Application.MilestoneEvents.Queries;
using TNTTBD.Services.Domain.Catechism;
using TNTTBD.Services.Domain.Common;

namespace TNTTBD.Services.Application.MilestoneEvents.Handlers;

public class GenerateMilestoneCodesHandler(IAuthorizationContext auth)
    : IRequestHandler<GenerateMilestoneCodesQuery, GenerateMilestoneCodesResultDto>
{
    public async Task<GenerateMilestoneCodesResultDto> Handle(GenerateMilestoneCodesQuery query, CancellationToken cancellationToken)
    {
        if (!await auth.IsAdminOrCatechistAsync(cancellationToken))
        {
            throw new ForbiddenException();
        }
        List<DateOnly> dates = query.Dates ?? [];
        if (dates.Count > SaveMilestoneEventHandler.MaxParticipants || query.StartIndex < 0)
        {
            throw new DomainException(DomainErrors.MilestoneEventInvalidTemplate);
        }
        MilestoneCodeTemplate template = MilestoneCodeTemplate.Parse(query.Template);
        return new GenerateMilestoneCodesResultDto
        {
            Codes = dates.Select((date, i) => template.Generate(query.StartIndex + i, date)).ToList()
        };
    }
}
