using MediatR;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Application.EucharisticYouths.Queries;

public record GetEucharisticYouthProfilePhotoQuery(Guid YouthId) : IRequest<ProfilePhotoFileDto?>;
