using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Exams.DTOs;
using TNTTBD.Services.Application.Exams.Interfaces;
using TNTTBD.Services.Application.Exams.Queries;

namespace TNTTBD.Services.Application.Exams.Handlers;

public class GetMyExamResultsHandler : IRequestHandler<GetMyExamResultsQuery, PaginatedResult<MyExamResultDto>>
{
    private readonly IExamQueryService _queryService;
    private readonly IAuthorizationContext _authContext;

    public GetMyExamResultsHandler(IExamQueryService queryService, IAuthorizationContext authContext)
    {
        _queryService = queryService;
        _authContext = authContext;
    }

    public async Task<PaginatedResult<MyExamResultDto>> Handle(GetMyExamResultsQuery query, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanReadYouthContactsAsync(query.YouthId, cancellationToken))
        {
            return new PaginatedResult<MyExamResultDto>
            {
                Page = query.Page,
                PageSize = query.PageSize ?? 0
            };
        }

        return await _queryService.GetMyResultsAsync(query.YouthId, query.Page, query.PageSize, cancellationToken);
    }
}
