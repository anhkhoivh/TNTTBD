using MediatR;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Application.Catechists.Queries;

public record GetCatechistPhotosQuery(Guid Id) : IRequest<List<PhotoReferenceDto>>;