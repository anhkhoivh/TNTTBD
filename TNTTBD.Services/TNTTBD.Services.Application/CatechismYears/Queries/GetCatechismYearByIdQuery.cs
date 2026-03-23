using MediatR;
using TNTTBD.Services.Application.CatechismYears.DTOs;

namespace TNTTBD.Services.Application.CatechismYears.Queries;

public record GetCatechismYearByIdQuery(Guid Id) : IRequest<CatechismYearDto>;