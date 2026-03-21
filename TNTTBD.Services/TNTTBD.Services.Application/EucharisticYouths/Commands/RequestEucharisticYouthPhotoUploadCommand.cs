using MediatR;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.EucharisticYouths.Commands;

public record RequestEucharisticYouthPhotoUploadCommand(
    Guid EucharisticYouthId,
    string FileName,
    string ContentType,
    PhotoPurpose Purpose,
    Guid UploadedBy,
    PhotoVisibility Visibility
) : IRequest<PhotoReferenceDto>;