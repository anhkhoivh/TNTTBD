namespace TNTTBD.Services.Application.Common.DTOs;

/// <summary>
/// Stored file of a member's current profile photo.
/// </summary>
public class ProfilePhotoFileDto
{
    public Guid PhotoId { get; set; }
    public required string StorageKey { get; set; }
    public required string ContentType { get; set; }
}
