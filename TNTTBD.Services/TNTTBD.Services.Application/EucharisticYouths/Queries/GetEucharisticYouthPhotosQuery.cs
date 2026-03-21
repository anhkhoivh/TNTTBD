using MediatR;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Application.EucharisticYouths.Queries;

public record GetEucharisticYouthPhotosQuery(Guid Id) : IRequest<List<PhotoReferenceDto>>;