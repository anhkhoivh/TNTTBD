using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.EucharisticYouths.Commands;

public record AddEucharisticYouthPhotoCommand(
    Guid EucharisticYouthId,
    string StorageKey,
    string ContentType,
    PhotoPurpose Purpose,
    DateTimeOffset UploadedAt,
    Guid UploadBy
);