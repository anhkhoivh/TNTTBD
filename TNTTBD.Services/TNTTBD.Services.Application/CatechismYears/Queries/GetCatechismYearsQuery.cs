using MediatR;
using TNTTBD.Services.Application.CatechismYears.DTOs;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Application.CatechismYears.Queries;

public record GetCatechismYearsQuery(int Page = 1, int PageSize = 50) : IRequest<PaginatedResult<CatechismYearDto>>;
