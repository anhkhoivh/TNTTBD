using MediatR;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.Exams.DTOs;

namespace TNTTBD.Services.Application.Exams.Queries;

public record GetExamsQuery(
    Guid? ClassId = null,
    int Page = 1,
    int PageSize = 50
) : IRequest<PaginatedResult<ExamDto>>;
