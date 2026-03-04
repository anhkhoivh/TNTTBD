using MediatR;
using TNTTBD.Services.Application.EucharisticYouths.DTOs;

namespace TNTTBD.Services.Application.EucharisticYouths.Queries;

public record GetEucharisticYouthByIdQuery(Guid Id) : IRequest<EucharisticYouthDto>;