using MediatR;
using TNTTBD.Services.Application.CatechismClasses.DTOs;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Application.CatechismClasses.Queries;

public record GetCatechismClassesQuery(Guid? YearId = null, int Page = 1, int PageSize = 50) : IRequest<PaginatedResult<CatechismClassDto>>;
