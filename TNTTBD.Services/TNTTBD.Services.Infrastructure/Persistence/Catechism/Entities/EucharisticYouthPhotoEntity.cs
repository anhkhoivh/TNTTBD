using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

public class EucharisticYouthPhotoEntity
{
    public Guid Id { get; set; }

    public Guid EucharisticYouthId { get; set; }
    public EucharisticYouthEntity EucharisticYouth { get; set; } = null!;

    public string StorageKey { get; set; } = null!;
    public string ContentType { get; set; } = null!;
    public PhotoPurpose Purpose { get; set; }
    public DateTimeOffset UploadedAt { get; set; }
    public Guid UploadedBy { get; set; }
}