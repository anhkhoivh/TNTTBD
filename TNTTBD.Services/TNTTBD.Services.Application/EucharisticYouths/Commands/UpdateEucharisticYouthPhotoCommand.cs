using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.EucharisticYouths.Commands;

public record UpdateEucharisticYouthPhotoCommand(
    Guid EucharisticYouthId,
    Guid PhotoId,
    PhotoPurpose Purpose
);
