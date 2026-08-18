using MediatR;
using TNTTBD.Services.Application.CatechismSessions.DTOs;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Application.CatechismSessions.Queries;

public record GetCatechismSessionsQuery(Guid? ClassId = null, int Page = 1, int? PageSize = null) : IRequest<PaginatedResult<CatechismSessionDto>>;
