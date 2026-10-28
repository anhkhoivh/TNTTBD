using MediatR;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.Catechists.Commands;

public record UpdateCatechistPhotoCommand(
    Guid CatechistId,
    Guid PhotoId,
    PhotoPurpose Purpose,
    PhotoVisibility Visibility
) : IRequest<bool>;
