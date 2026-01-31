using TNTTBD.Services.Domain.Common;
using TNTTBD.Services.Domain.Enums;

public sealed class PhotoReference : ValueObject
{
    #region Constructors

    public PhotoReference()
    {
        StorageKey = string.Empty;
        ContentType = string.Empty;
    }

    public PhotoReference(
        string storageKey,
        string contentType,
        PhotoPurpose purpose,
        Guid uploadedBy,
        PhotoVisibility visibility)
    {
        StorageKey = storageKey;
        ContentType = contentType;
        Purpose = purpose;
        UploadedBy = uploadedBy;
        Visibility = visibility;
        UploadedAt = DateTime.UtcNow;
    }

    #endregion

    #region Properties

    public string StorageKey { get; }
    public string ContentType { get; }
    public PhotoPurpose Purpose { get; }
    public DateTime UploadedAt { get; }
    public Guid UploadedBy { get; }
    public PhotoVisibility Visibility { get; }
    
    #endregion

    #region Methods

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return StorageKey;
        yield return Purpose;
    }

    #endregion
}