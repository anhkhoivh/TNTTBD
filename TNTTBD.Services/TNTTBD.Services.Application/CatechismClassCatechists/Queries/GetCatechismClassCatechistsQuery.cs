using MediatR;
using TNTTBD.Services.Application.CatechismClassCatechists.DTOs;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Application.CatechismClassCatechists.Queries;

public record GetCatechismClassCatechistsQuery(
    Guid? ClassId = null,
    Guid? CatechistId = null,
    int Page = 1,
    int? PageSize = null
) : IRequest<PaginatedResult<CatechismClassCatechistDto>>;
