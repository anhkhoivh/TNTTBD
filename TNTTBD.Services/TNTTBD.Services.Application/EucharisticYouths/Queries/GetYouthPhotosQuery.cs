using MediatR;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.EucharisticYouths.DTOs;

namespace TNTTBD.Services.Application.EucharisticYouths.Queries;

public record GetYouthPhotosQuery(Guid Id) : IRequest<List<PhotoReferenceDto>>;