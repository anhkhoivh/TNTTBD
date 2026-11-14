using MediatR;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Common.Exceptions;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.MilestoneEvents.DTOs;
using TNTTBD.Services.Application.MilestoneEvents.Interfaces;
using TNTTBD.Services.Application.MilestoneEvents.Queries;

namespace TNTTBD.Services.Application.MilestoneEvents.Handlers;

public class GetMilestoneEventCandidatesHandler(
    IMilestoneEventQueryService queryService,
    IAuthorizationContext auth) : IRequestHandler<GetMilestoneEventCandidatesQuery, PaginatedResult<MilestoneEventCandidateDto>>
{
    public async Task<PaginatedResult<MilestoneEventCandidateDto>> Handle(GetMilestoneEventCandidatesQuery query, CancellationToken cancellationToken)
    {
        if (!await auth.IsAdminOrCatechistAsync(cancellationToken))
        {
            throw new ForbiddenException();
        }
        int? pageSize = query.PageSize is null or < 1 ? 20 : Math.Min(query.PageSize.Value, 100);
        return await queryService.GetCandidatesAsync(query.Type, query.Date, query.Search, query.ClassId, query.CatechismYearId, Math.Max(1, query.Page), pageSize, cancellationToken);
    }
}
