using MediatR;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.EucharisticYouths.Commands;

public record AddPhotoCommand(
    Guid EucharisticYouthId,
    string StorageKey,
    string ContentType,
    PhotoPurpose Purpose,
    DateTimeOffset UploadedAt,
    Guid UploadBy,
    PhotoVisibility Visibility
) : IRequest;