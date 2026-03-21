using MediatR;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.Catechists.Commands;

public record RequestCatechistPhotoUploadCommand(
    Guid CatechistId,
    string FileName,
    string ContentType,
    PhotoPurpose Purpose,
    Guid UploadedBy,
    PhotoVisibility Visibility
) : IRequest<PhotoReferenceDto>;