namespace TNTTBD.Services.Application.Common.DTOs;

/// <summary>
/// Photo Reference Dto
/// </summary>
public class PhotoReferenceDto
{
    public required string StorageKey { get; set; }
    public required string ContentType { get; set; }
    public required string Purpose { get; set; }
    public DateTime UploadedAt { get; set; }
    public Guid UploadedBy { get; set; }
    public required string Visibility { get; set; }
}