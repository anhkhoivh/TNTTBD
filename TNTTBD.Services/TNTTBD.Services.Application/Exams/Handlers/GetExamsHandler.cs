using MediatR;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.Exams.DTOs;
using TNTTBD.Services.Application.Exams.Interfaces;
using TNTTBD.Services.Application.Exams.Queries;

namespace TNTTBD.Services.Application.Exams.Handlers;

public class GetExamsHandler : IRequestHandler<GetExamsQuery, PaginatedResult<ExamDto>>
{
    private readonly IExamQueryService _queryService;

    public GetExamsHandler(IExamQueryService queryService)
    {
        _queryService = queryService;
    }

    public async Task<PaginatedResult<ExamDto>> Handle(GetExamsQuery query, CancellationToken cancellationToken)
    {
        return await _queryService.GetAllAsync(query.ClassId, query.Page, query.PageSize, cancellationToken);
    }
}
