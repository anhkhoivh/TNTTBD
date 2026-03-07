using TNTTBD.Services.Domain.Common;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Domain.Catechism.Members;

/// <summary>
/// Photo reference
/// </summary>
public class PhotoReference : DomainEntity<Guid>
{
    #region Constructors

    public PhotoReference(
        Guid id,
        string storageKey,
        string contentType,
        PhotoPurpose purpose,
        Guid uploadedBy,
        DateTimeOffset uploadedAt,
        PhotoVisibility visibility) : base(id)
    {
        StorageKey = storageKey;
        ContentType = contentType;
        Purpose = purpose;
        UploadedBy = uploadedBy;
        Visibility = visibility;
        UploadedAt = uploadedAt;
    }

    #endregion

    #region Properties

    public string StorageKey { get; }
    public string ContentType { get; }
    public PhotoPurpose Purpose { get; }
    public DateTimeOffset UploadedAt { get; }
    public Guid UploadedBy { get; }
    public PhotoVisibility Visibility { get; }
    
    #endregion

    #region Methods

    #endregion
}