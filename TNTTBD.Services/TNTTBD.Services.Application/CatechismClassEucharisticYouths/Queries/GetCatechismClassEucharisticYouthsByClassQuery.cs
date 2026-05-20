using MediatR;
using TNTTBD.Services.Application.CatechismClassEucharisticYouths.DTOs;

namespace TNTTBD.Services.Application.CatechismClassEucharisticYouths.Queries;

public record GetCatechismClassEucharisticYouthsByClassQuery(Guid CatechismClassId) : IRequest<CatechismClassEucharisticYouthDto>;