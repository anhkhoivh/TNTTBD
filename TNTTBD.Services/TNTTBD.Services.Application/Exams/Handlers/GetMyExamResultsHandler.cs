using MediatR;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.Exams.DTOs;
using TNTTBD.Services.Application.Exams.Interfaces;
using TNTTBD.Services.Application.Exams.Queries;

namespace TNTTBD.Services.Application.Exams.Handlers;

public class GetMyExamResultsHandler : IRequestHandler<GetMyExamResultsQuery, PaginatedResult<MyExamResultDto>>
{
    private readonly IExamQueryService _queryService;

    public GetMyExamResultsHandler(IExamQueryService queryService)
    {
        _queryService = queryService;
    }

    public async Task<PaginatedResult<MyExamResultDto>> Handle(GetMyExamResultsQuery query, CancellationToken cancellationToken)
    {
        return await _queryService.GetMyResultsAsync(query.YouthId, query.Page, query.PageSize, cancellationToken);
    }
}
