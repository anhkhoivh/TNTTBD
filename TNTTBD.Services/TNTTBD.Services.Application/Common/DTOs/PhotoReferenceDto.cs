namespace TNTTBD.Services.Application.Common.DTOs;

/// <summary>
/// Photo Reference Dto
/// </summary>
public class PhotoReferenceDto
{
    public Guid PhotoId { get; set; }
    public required string ContentType { get; set; }
    public required string Url { get; set; }
    public required string Purpose { get; set; }
    public Guid UploadedBy { get; set; }
}