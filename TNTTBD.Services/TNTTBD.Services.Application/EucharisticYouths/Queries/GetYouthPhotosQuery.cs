using MediatR;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Application.EucharisticYouths.Queries;

public record GetYouthPhotosQuery(Guid Id) : IRequest<List<PhotoReferenceDto>>;