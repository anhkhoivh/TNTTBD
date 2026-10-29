using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TNTTBD.Services.Application.Auth.Interfaces;

namespace TNTTBD.Services.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/files")]
public class FileUploadController : ControllerBase
{
    private readonly string _basePath;
    private readonly IAuthorizationContext _authContext;
    private const long MaxFileSize = 20 * 1024 * 1024;
    private const long MaxRequestSize = 22 * 1024 * 1024;

    private static readonly Regex StorageKeyPattern = new(
        @"^(catechists|youths)/[a-f0-9\-]{36}/photos/[a-f0-9\-]{36}\.(jpg|jpeg|png|gif|webp)$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private static readonly Dictionary<string, byte[]> ImageSignatures = new()
    {
        { ".jpg",  [0xFF, 0xD8, 0xFF] },
        { ".jpeg", [0xFF, 0xD8, 0xFF] },
        { ".png",  [0x89, 0x50, 0x4E, 0x47] },
        { ".gif",  [0x47, 0x49, 0x46, 0x38] },
    };

    public FileUploadController(IConfiguration configuration, IAuthorizationContext authContext)
    {
        _basePath = configuration["FileStorage:BasePath"]
            ?? Path.Combine(Directory.GetCurrentDirectory(), "uploads");
        _authContext = authContext;
    }

    [HttpPost("upload/{**storageKey}")]
    [RequestSizeLimit(MaxRequestSize)]
    public async Task<IActionResult> Upload(string storageKey, IFormFile file, CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file provided.");
        }

        if (file.Length > MaxFileSize)
        {
            return BadRequest("File size exceeds 20MB limit.");
        }

        if (!file.ContentType.StartsWith("image/"))
        {
            return BadRequest("Only image files are allowed.");
        }

        // Storage key format validation
        if (!StorageKeyPattern.IsMatch(storageKey))
        {
            return BadRequest("Invalid storage key format.");
        }

        // Path traversal prevention
        if (storageKey.Contains("..") || Path.IsPathRooted(storageKey))
        {
            return BadRequest("Invalid storage key.");
        }

        // Ownership validation
        var ownershipResult = await ValidateOwnership(storageKey, cancellationToken);
        if (ownershipResult != null)
        {
            return ownershipResult;
        }

        // Magic byte validation
        var magicResult = await ValidateImageSignature(file);
        if (magicResult != null)
        {
            return magicResult;
        }

        var filePath = Path.Combine(_basePath, storageKey.Replace('/', Path.DirectorySeparatorChar));
        var directory = Path.GetDirectoryName(filePath)!;

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream, cancellationToken);

        return Ok(new { url = $"/api/files/{storageKey}" });
    }

    [HttpGet("{**storageKey}")]
    public IActionResult GetFile(string storageKey)
    {
        if (!StorageKeyPattern.IsMatch(storageKey))
        {
            return BadRequest("Invalid storage key format.");
        }

        if (storageKey.Contains("..") || Path.IsPathRooted(storageKey))
        {
            return BadRequest("Invalid storage key.");
        }

        var filePath = Path.Combine(_basePath, storageKey.Replace('/', Path.DirectorySeparatorChar));

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        var extension = Path.GetExtension(filePath).ToLowerInvariant();
        var contentType = extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".webp" => "image/webp",
            _ => "application/octet-stream"
        };

        return PhysicalFile(filePath, contentType);
    }

    private async Task<IActionResult?> ValidateOwnership(string storageKey, CancellationToken cancellationToken)
    {
        if (_authContext.GetCurrentUserId() == null)
        {
            return Unauthorized();
        }

        // Parse entity type and ID from storageKey: "{type}/{entityId}/photos/..."
        var segments = storageKey.Split('/');
        var entityType = segments[0];
        if (!Guid.TryParse(segments[1], out var entityId))
        {
            return BadRequest("Invalid storage key format.");
        }

        bool canManage = entityType == "catechists"
            ? await _authContext.CanManageCatechistPhotosAsync(entityId, cancellationToken)
            : await _authContext.CanManageYouthPhotosAsync(entityId, cancellationToken);

        return canManage ? null : Forbid();
    }

    private static async Task<BadRequestObjectResult?> ValidateImageSignature(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        // WebP: check RIFF....WEBP header
        if (extension is ".webp")
        {
            if (file.Length < 12)
            {
                return new BadRequestObjectResult("Invalid image file.");
            }

            var header = new byte[12];
            using var s = file.OpenReadStream();
            await s.ReadExactlyAsync(header.AsMemory(0, 12));
            if (header[0] != 0x52 || header[1] != 0x49 || header[2] != 0x46 || header[3] != 0x46 ||
                header[8] != 0x57 || header[9] != 0x45 || header[10] != 0x42 || header[11] != 0x50)
            {
                return new BadRequestObjectResult("Invalid image file.");
            }

            return null;
        }

        if (!ImageSignatures.TryGetValue(extension, out var signature))
        {
            return new BadRequestObjectResult("Unsupported image format.");
        }

        if (file.Length < signature.Length)
        {
            return new BadRequestObjectResult("Invalid image file.");
        }

        var buffer = new byte[signature.Length];
        using var stream = file.OpenReadStream();
        await stream.ReadExactlyAsync(buffer.AsMemory(0, signature.Length));

        if (!buffer.AsSpan().SequenceEqual(signature))
        {
            return new BadRequestObjectResult("File content does not match image format.");
        }

        return null;
    }
}
