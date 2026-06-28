using MediatR;
using TNTTBD.Services.Application.CatechismClassCatechists.DTOs;

namespace TNTTBD.Services.Application.CatechismClassCatechists.Queries;

public record GetCatechismClassCatechistByIdQuery(Guid Id) : IRequest<CatechismClassCatechistDto>;
