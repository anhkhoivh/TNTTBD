using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.Catechists.Commands;

public record AddCatechistPhotoCommand(
    Guid CatechistId,
    string StorageKey,
    string ContentType,
    PhotoPurpose Purpose,
    DateTimeOffset UploadedAt,
    Guid UploadBy
);