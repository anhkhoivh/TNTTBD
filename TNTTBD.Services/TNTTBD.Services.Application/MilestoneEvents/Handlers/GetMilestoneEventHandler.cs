using MediatR;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.MilestoneEvents.DTOs;
using TNTTBD.Services.Application.MilestoneEvents.Interfaces;
using TNTTBD.Services.Application.MilestoneEvents.Queries;

namespace TNTTBD.Services.Application.MilestoneEvents.Handlers;

public class GetMilestoneEventHandler(
    IMilestoneEventQueryService queryService,
    IAuthorizationContext auth) : IRequestHandler<GetMilestoneEventQuery, MilestoneEventDetailDto?>
{
    public async Task<MilestoneEventDetailDto?> Handle(GetMilestoneEventQuery query, CancellationToken cancellationToken)
    {
        MilestoneEventDetailDto? detail = await queryService.GetDetailAsync(query.Id, cancellationToken);
        if (detail != null && !await auth.IsAdminOrCatechistAsync(cancellationToken))
        {
            // Parents / youths see who took part, not other children's personal data or godparent phone numbers.
            detail.CodeTemplate = null;
            foreach (MilestoneEventParticipantDto p in detail.Participants)
            {
                p.YouthCode = string.Empty;
                p.DateOfBirth = null;
                p.BaptismDate = null;
                p.DocumentCode = null;
                p.Sponsor = null;
                p.Note = null;
                p.Warnings = [];
            }
        }
        return detail;
    }
}
