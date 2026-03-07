namespace TNTTBD.Services.Application.Common.DTOs;

/// <summary>
/// Response
/// </summary>
public class UploadUrlReponse
{
    public Guid PhotoId { get; set; }
    public required string UploadUrl { get; set; }
    public required string StorageKey { get; set; }
}