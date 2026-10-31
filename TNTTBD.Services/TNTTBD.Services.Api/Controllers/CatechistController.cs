using MediatR;
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

    private readonly IMediator _mediator;
    private readonly IFileStorage _storage;
    private readonly ILogger<CatechistController> _logger;

    #endregion

    #region Constructors

    public CatechistController(IMediator mediator, IFileStorage storage, ILogger<CatechistController> logger)
    {
        _mediator = mediator;
        _storage = storage;
        _logger = logger;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Resolve the calling catechist's class for the current catechism year.
    /// </summary>
    [HttpGet("current-class")]
    public async Task<IActionResult> GetCurrentClass(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCatechistCurrentClassQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Create
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCatechistCommand command, CancellationToken cancellationToken)
    {
        Guid? id = await _mediator.Send(command, cancellationToken);

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
    public async Task<IActionResult> GetAll([FromQuery] GetCatechistsQuery query, CancellationToken cancellationToken)
    {
        PaginatedResult<CatechistDto> result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get by id
    /// </summary>
    [HttpGet("detail")]
    public async Task<IActionResult> GetById([FromQuery] GetCatechistByIdQuery query, CancellationToken cancellationToken)
    {
        CatechistDto? result = await _mediator.Send(query, cancellationToken);

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
    public async Task<IActionResult> Update(UpdateCatechistCommand command, CancellationToken cancellationToken)
    {
        return await _mediator.Send(command, cancellationToken) ? Ok() : Forbid();
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(RemoveCatechistCommand command, CancellationToken cancellationToken)
    {
        return await _mediator.Send(command, cancellationToken) ? Ok() : Forbid();
    }

    /// <summary>
    /// Add Patron Saints
    /// </summary>
    [HttpPost("patronSaints")]
    public async Task<IActionResult> AddPatronSaint(AddCatechistPatronSaintCommand command, CancellationToken cancellationToken)
    {
        return await _mediator.Send(command, cancellationToken) ? Ok() : Forbid();
    }

    /// <summary>
    /// Add Patron Saints
    /// </summary>
    [HttpDelete("patronSaints")]
    public async Task<IActionResult> RemovePatronSaint(RemoveCatechistPatronSaintCommand command, CancellationToken cancellationToken)
    {
        return await _mediator.Send(command, cancellationToken) ? Ok() : Forbid();
    }

    [HttpPost("photos/upload-url")]
    public async Task<IActionResult> RequestUploadUrl(RequestCatechistPhotoUploadCommand command, CancellationToken cancellationToken)
    {
        PhotoReferenceDto? result = await _mediator.Send(command, cancellationToken);

        return result == null ? Forbid() : Ok(result);
    }

    [HttpGet("photos")]
    public async Task<IActionResult> GetPhotos(GetCatechistPhotosQuery query, CancellationToken cancellationToken)
    {
        List<PhotoReferenceDto> result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Current profile photo file (204 when the catechist has none)
    /// </summary>
    [HttpGet("profile-photo")]
    public async Task<IActionResult> GetProfilePhoto([FromQuery] Guid catechistId, CancellationToken cancellationToken)
    {
        ProfilePhotoFileDto? photo = await _mediator.Send(new GetCatechistProfilePhotoQuery(catechistId), cancellationToken);
        return this.ProfilePhotoResult(photo, _storage);
    }

    [HttpGet("photos/permissions")]
    public async Task<IActionResult> GetPhotoPermissions([FromQuery] Guid catechistId, CancellationToken cancellationToken)
    {
        bool canManage = await _mediator.Send(new GetCatechistPhotoPermissionsQuery(catechistId), cancellationToken);
        return Ok(new { canManage });
    }

    [HttpPatch("photos")]
    public async Task<IActionResult> UpdatePhoto(UpdateCatechistPhotoCommand command, CancellationToken cancellationToken)
    {
        return await _mediator.Send(command, cancellationToken) ? Ok() : Forbid();
    }

    [HttpDelete("photos")]
    public async Task<IActionResult> RemovePhoto(RemoveCatechistPhotoCommand command, CancellationToken cancellationToken)
    {
        string? storageKey = await _mediator.Send(command, cancellationToken);
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