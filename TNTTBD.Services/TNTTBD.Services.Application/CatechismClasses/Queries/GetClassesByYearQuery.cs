using MediatR;
using TNTTBD.Services.Application.CatechismClasses.DTOs;

namespace TNTTBD.Services.Application.CatechismClasses.Queries;

public record GetClassesByYearQuery(Guid CatechismYearId) : IRequest<List<CatechismClassDto>>;