using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Exams.DTOs;
using TNTTBD.Services.Application.Exams.Interfaces;
using TNTTBD.Services.Application.Exams.Queries;
using TNTTBD.Services.Domain.Common;

namespace TNTTBD.Services.Application.Exams.Handlers;

public class GetExamByIdHandler : IRequestHandler<GetExamByIdQuery, ExamDto>
{
    private readonly IExamQueryService _queryService;

    public GetExamByIdHandler(IExamQueryService queryService)
    {
        _queryService = queryService;
    }

    public async Task<ExamDto> Handle(GetExamByIdQuery query, CancellationToken cancellationToken)
    {
        return await _queryService.GetByIdAsync(query.Id, cancellationToken)
            ?? throw new DomainException(DomainErrors.ValidationNotFound);
    }
}
