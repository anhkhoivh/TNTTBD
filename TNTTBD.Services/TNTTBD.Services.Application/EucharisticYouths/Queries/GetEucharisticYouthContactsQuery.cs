using MediatR;
using TNTTBD.Services.Application.EucharisticYouths.DTOs;

namespace TNTTBD.Services.Application.EucharisticYouths.Queries;

public record GetEucharisticYouthContactsQuery(Guid EucharisticYouthId) : IRequest<List<EucharisticYouthContactDto>>;
