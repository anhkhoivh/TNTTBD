using MediatR;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.Exams.DTOs;

namespace TNTTBD.Services.Application.Exams.Queries;

public record GetMyExamResultsQuery(
    Guid YouthId,
    int Page = 1,
    int? PageSize = null
) : IRequest<PaginatedResult<MyExamResultDto>>;
