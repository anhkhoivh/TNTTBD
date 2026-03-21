using MediatR;
using TNTTBD.Services.Application.Catechists.DTOs;

namespace TNTTBD.Services.Application.Catechists.Queries;

public record GetCatechistByIdQuery(Guid Id) : IRequest<CatechistDto>;