using MediatR;
using TNTTBD.Services.Application.Catechists.DTOs;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Application.Catechists.Queries;

public record GetCatechistsQuery(int Page = 1, int? PageSize = null, string? Search = null) : IRequest<PaginatedResult<CatechistDto>>;
