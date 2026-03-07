using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

public class CatechistPhotoEntity
{
    public Guid Id { get; set; }

    public Guid CatechistId { get; set; }

    public string StorageKey { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public PhotoPurpose Purpose { get; set; }
    public DateTimeOffset UploadedAt { get; set; }
    public Guid UploadedBy { get; set; }
    public PhotoVisibility Visibility { get; set; }
}