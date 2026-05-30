using MediatR;
using TNTTBD.Services.Application.CatechismClassCatechists.DTOs;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Application.CatechismClassCatechists.Queries;

public record GetCatechismClassCatechistsQuery(
    Guid? ClassId = null,
    int Page = 1,
    int PageSize = 50
) : IRequest<PaginatedResult<CatechismClassCatechistDto>>;
