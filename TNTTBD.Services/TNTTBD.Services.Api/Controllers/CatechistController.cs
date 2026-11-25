using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TNTTBD.Services.Api.Common;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Catechists.Commands;
using TNTTBD.Services.Application.Catechists.DTOs;
using TNTTBD.Services.Application.Catechists.Queries;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/catechists")]
public class CatechistController : Controller
{
    #region Fields

    private readonly IFileStorage _storage;
    private readonly ILogger<CatechistController> _logger;

    #endregion

    #region Constructors

    public CatechistController(IFileStorage storage, ILogger<CatechistController> logger)
    {
        _storage = storage;
        _logger = logger;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Resolve the calling catechist's class for the current catechism year.
    /// </summary>
    [HttpGet("current-class")]
    public async Task<IActionResult> GetCurrentClass(
        [FromServices] IRequestHandler<GetCatechistCurrentClassQuery, CatechistCurrentClassDto?> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new GetCatechistCurrentClassQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Create
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateCatechistCommand command,
        [FromServices] IRequestHandler<CreateCatechistCommand, Guid?> handler,
        CancellationToken cancellationToken)
    {
        Guid? id = await handler.Handle(command, cancellationToken);

        return id switch
        {
            null => Forbid(),
            _ => CreatedAtAction(nameof(GetById), new { id }, id)
        };
    }

    /// <summary>
    /// Get list (paginated)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetCatechistsQuery query,
        [FromServices] IRequestHandler<GetCatechistsQuery, PaginatedResult<CatechistDto>> handler,
        CancellationToken cancellationToken)
    {
        PaginatedResult<CatechistDto> result = await handler.Handle(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get by id
    /// </summary>
    [HttpGet("detail")]
    public async Task<IActionResult> GetById(
        [FromQuery] GetCatechistByIdQuery query,
        [FromServices] IRequestHandler<GetCatechistByIdQuery, CatechistDto> handler,
        CancellationToken cancellationToken)
    {
        CatechistDto? result = await handler.Handle(query, cancellationToken);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    /// <summary>
    /// Update
    /// </summary>
    [HttpPatch]
    public async Task<IActionResult> Update(
        UpdateCatechistCommand command,
        [FromServices] IRequestHandler<UpdateCatechistCommand, bool> handler,
        CancellationToken cancellationToken)
    {
        return await handler.Handle(command, cancellationToken) ? Ok() : Forbid();
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(
        RemoveCatechistCommand command,
        [FromServices] IRequestHandler<RemoveCatechistCommand, bool> handler,
        CancellationToken cancellationToken)
    {
        return await handler.Handle(command, cancellationToken) ? Ok() : Forbid();
    }

    /// <summary>
    /// Add Patron Saints
    /// </summary>
    [HttpPost("patronSaints")]
    public async Task<IActionResult> AddPatronSaint(
        AddCatechistPatronSaintCommand command,
        [FromServices] IRequestHandler<AddCatechistPatronSaintCommand, bool> handler,
        CancellationToken cancellationToken)
    {
        return await handler.Handle(command, cancellationToken) ? Ok() : Forbid();
    }

    /// <summary>
    /// Add Patron Saints
    /// </summary>
    [HttpDelete("patronSaints")]
    public async Task<IActionResult> RemovePatronSaint(
        RemoveCatechistPatronSaintCommand command,
        [FromServices] IRequestHandler<RemoveCatechistPatronSaintCommand, bool> handler,
        CancellationToken cancellationToken)
    {
        return await handler.Handle(command, cancellationToken) ? Ok() : Forbid();
    }

    [HttpPost("photos/upload-url")]
    public async Task<IActionResult> RequestUploadUrl(
        RequestCatechistPhotoUploadCommand command,
        [FromServices] IRequestHandler<RequestCatechistPhotoUploadCommand, PhotoReferenceDto?> handler,
        CancellationToken cancellationToken)
    {
        PhotoReferenceDto? result = await handler.Handle(command, cancellationToken);

        return result == null ? Forbid() : Ok(result);
    }

    [HttpGet("photos")]
    public async Task<IActionResult> GetPhotos(
        GetCatechistPhotosQuery query,
        [FromServices] IRequestHandler<GetCatechistPhotosQuery, List<PhotoReferenceDto>> handler,
        CancellationToken cancellationToken)
    {
        List<PhotoReferenceDto> result = await handler.Handle(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Current profile photo file (204 when the catechist has none)
    /// </summary>
    [HttpGet("profile-photo")]
    public async Task<IActionResult> GetProfilePhoto(
        [FromQuery] Guid catechistId,
        [FromServices] IRequestHandler<GetCatechistProfilePhotoQuery, ProfilePhotoFileDto?> handler,
        CancellationToken cancellationToken)
    {
        ProfilePhotoFileDto? photo = await handler.Handle(new GetCatechistProfilePhotoQuery(catechistId), cancellationToken);
        return this.ProfilePhotoResult(photo, _storage);
    }

    [HttpGet("photos/permissions")]
    public async Task<IActionResult> GetPhotoPermissions(
        [FromQuery] Guid catechistId,
        [FromServices] IRequestHandler<GetCatechistPhotoPermissionsQuery, bool> handler,
        CancellationToken cancellationToken)
    {
        bool canManage = await handler.Handle(new GetCatechistPhotoPermissionsQuery(catechistId), cancellationToken);
        return Ok(new { canManage });
    }

    [HttpPatch("photos")]
    public async Task<IActionResult> UpdatePhoto(
        UpdateCatechistPhotoCommand command,
        [FromServices] IRequestHandler<UpdateCatechistPhotoCommand, bool> handler,
        CancellationToken cancellationToken)
    {
        return await handler.Handle(command, cancellationToken) ? Ok() : Forbid();
    }

    [HttpDelete("photos")]
    public async Task<IActionResult> RemovePhoto(
        RemoveCatechistPhotoCommand command,
        [FromServices] IRequestHandler<RemoveCatechistPhotoCommand, string?> handler,
        CancellationToken cancellationToken)
    {
        string? storageKey = await handler.Handle(command, cancellationToken);
        if (storageKey == null)
        {
            return Forbid();
        }

        await DeletePhotoFileAfterCommit(storageKey, command.PhotoId);
        return Ok();
    }

    private async Task DeletePhotoFileAfterCommit(string storageKey, Guid photoId)
    {
        try
        {
            await _storage.DeleteAsync(storageKey);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to delete file {StorageKey} for removed photo {PhotoId}", storageKey, photoId);
        }
    }

    #endregion
}