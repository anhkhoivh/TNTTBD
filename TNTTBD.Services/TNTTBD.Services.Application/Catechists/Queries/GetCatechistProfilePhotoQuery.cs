using MediatR;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Application.Catechists.Queries;

public record GetCatechistProfilePhotoQuery(Guid CatechistId) : IRequest<ProfilePhotoFileDto?>;
