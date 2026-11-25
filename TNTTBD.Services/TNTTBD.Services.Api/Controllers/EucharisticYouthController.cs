using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TNTTBD.Services.Api.Common;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.CatechismClassEucharisticYouths.DTOs;
using TNTTBD.Services.Application.CatechismClassEucharisticYouths.Queries;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.Contacts.Commands;
using TNTTBD.Services.Application.EucharisticYouths.Commands;
using TNTTBD.Services.Application.EucharisticYouths.DTOs;
using TNTTBD.Services.Application.EucharisticYouths.Queries;

namespace TNTTBD.Services.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/youths")]
public class EucharisticYouthController : Controller
{
    #region Fields

    private readonly IFileStorage _storage;
    private readonly ILogger<EucharisticYouthController> _logger;

    #endregion

    #region Constructors

    public EucharisticYouthController(IFileStorage storage, ILogger<EucharisticYouthController> logger)
    {
        _storage = storage;
        _logger = logger;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Create
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateEucharisticYouthCommand command,
        [FromServices] IRequestHandler<CreateEucharisticYouthCommand, Guid?> handler,
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
    /// Get list (paginated, optional search filter)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetEucharisticYouthsQuery query,
        [FromServices] IRequestHandler<GetEucharisticYouthsQuery, PaginatedResult<EucharisticYouthDto>> handler,
        CancellationToken cancellationToken)
    {
        PaginatedResult<EucharisticYouthDto> result = await handler.Handle(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get by id
    /// </summary>
    [HttpGet("detail")]
    public async Task<IActionResult> GetById(
        [FromQuery] GetEucharisticYouthByIdQuery query,
        [FromServices] IRequestHandler<GetEucharisticYouthByIdQuery, EucharisticYouthDto> handler,
        CancellationToken cancellationToken)
    {
        EucharisticYouthDto? result = await handler.Handle(query, cancellationToken);

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
        UpdateEucharisticYouthCommand command,
        [FromServices] IRequestHandler<UpdateEucharisticYouthCommand, bool> handler,
        CancellationToken cancellationToken)
    {
        return await handler.Handle(command, cancellationToken) ? Ok() : Forbid();
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(
        RemoveEucharisticYouthCommand command,
        [FromServices] IRequestHandler<RemoveEucharisticYouthCommand, bool> handler,
        CancellationToken cancellationToken)
    {
        return await handler.Handle(command, cancellationToken) ? Ok() : Forbid();
    }

    /// <summary>
    /// Add milestone
    /// </summary>
    [HttpPost("milestones")]
    public async Task<IActionResult> AddMilestone(
        AddEucharisticYouthMilestoneCommand command,
        [FromServices] IRequestHandler<AddEucharisticYouthMilestoneCommand, bool> handler,
        CancellationToken cancellationToken)
    {
        return await handler.Handle(command, cancellationToken) ? Ok() : Forbid();
    }

    /// <summary>
    /// Add milestone
    /// </summary>
    [HttpPut("milestones")]
    public async Task<IActionResult> UpdateMilestone(
        UpdateEucharisticYouthMilestoneCommand command,
        [FromServices] IRequestHandler<UpdateEucharisticYouthMilestoneCommand, bool> handler,
        CancellationToken cancellationToken)
    {
        return await handler.Handle(command, cancellationToken) ? Ok() : Forbid();
    }

    [HttpDelete("milestones")]
    public async Task<IActionResult> RemoveMilestone(
        RemoveEucharisticYouthMilestoneCommand command,
        [FromServices] IRequestHandler<RemoveEucharisticYouthMilestoneCommand, bool> handler,
        CancellationToken cancellationToken)
    {
        return await handler.Handle(command, cancellationToken) ? Ok() : Forbid();
    }

    /// <summary>
    /// Add Patron Saints
    /// </summary>
    [HttpPost("patronSaints")]
    public async Task<IActionResult> AddPatronSaint(
        AddEucharisticYouthPatronSaintCommand command,
        [FromServices] IRequestHandler<AddEucharisticYouthPatronSaintCommand, bool> handler,
        CancellationToken cancellationToken)
    {
        return await handler.Handle(command, cancellationToken) ? Ok() : Forbid();
    }

    /// <summary>
    /// Add Patron Saints
    /// </summary>
    [HttpDelete("patronSaints")]
    public async Task<IActionResult> RemovePatronSaint(
        RemoveEucharisticYouthPatronSaintCommand command,
        [FromServices] IRequestHandler<RemoveEucharisticYouthPatronSaintCommand, bool> handler,
        CancellationToken cancellationToken)
    {
        return await handler.Handle(command, cancellationToken) ? Ok() : Forbid();
    }

    /// <summary>
    /// Request photo upload
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("photos/upload-url")]
    public async Task<IActionResult> RequestUploadUrl(
        RequestEucharisticYouthPhotoUploadCommand command,
        [FromServices] IRequestHandler<RequestEucharisticYouthPhotoUploadCommand, PhotoReferenceDto?> handler,
        CancellationToken cancellationToken)
    {
        PhotoReferenceDto? result = await handler.Handle(command, cancellationToken);

        return result == null ? Forbid() : Ok(result);
    }

    /// <summary>
    /// Get list of photos
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("photos")]
    public async Task<IActionResult> GetPhotos(
        GetEucharisticYouthPhotosQuery query,
        [FromServices] IRequestHandler<GetEucharisticYouthPhotosQuery, List<PhotoReferenceDto>> handler,
        CancellationToken cancellationToken)
    {
        List<PhotoReferenceDto> result = await handler.Handle(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Current profile photo file (204 when the youth has none)
    /// </summary>
    [HttpGet("profile-photo")]
    public async Task<IActionResult> GetProfilePhoto(
        [FromQuery] Guid youthId,
        [FromServices] IRequestHandler<GetEucharisticYouthProfilePhotoQuery, ProfilePhotoFileDto?> handler,
        CancellationToken cancellationToken)
    {
        ProfilePhotoFileDto? photo = await handler.Handle(new GetEucharisticYouthProfilePhotoQuery(youthId), cancellationToken);
        return this.ProfilePhotoResult(photo, _storage);
    }

    /// <summary>
    /// Get the current user's photo management permission
    /// </summary>
    [HttpGet("photos/permissions")]
    public async Task<IActionResult> GetPhotoPermissions(
        [FromQuery] Guid youthId,
        [FromServices] IRequestHandler<GetEucharisticYouthPhotoPermissionsQuery, bool> handler,
        CancellationToken cancellationToken)
    {
        bool canManage = await handler.Handle(new GetEucharisticYouthPhotoPermissionsQuery(youthId), cancellationToken);
        return Ok(new { canManage });
    }

    /// <summary>
    /// Update photo purpose
    /// </summary>
    [HttpPatch("photos")]
    public async Task<IActionResult> UpdatePhoto(
        UpdateEucharisticYouthPhotoCommand command,
        [FromServices] IRequestHandler<UpdateEucharisticYouthPhotoCommand, bool> handler,
        CancellationToken cancellationToken)
    {
        return await handler.Handle(command, cancellationToken) ? Ok() : Forbid();
    }

    /// <summary>
    /// Remove photo
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("photos")]
    public async Task<IActionResult> RemovePhoto(
        RemoveEucharisticYouthPhotoCommand command,
        [FromServices] IRequestHandler<RemoveEucharisticYouthPhotoCommand, string?> handler,
        CancellationToken cancellationToken)
    {
        string? storageKey = await handler.Handle(command, cancellationToken);
        if (storageKey == null)
        {
            return Forbid();
        }

        try
        {
            await _storage.DeleteAsync(storageKey);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to delete file {StorageKey} for removed photo {PhotoId}", storageKey, command.PhotoId);
        }

        return Ok();
    }

    [HttpGet("enrollments")]
    public async Task<IActionResult> GetEnrollments(
        [FromQuery] Guid youthId,
        [FromServices] IRequestHandler<GetCatechismClassEucharisticYouthsByYouthQuery, List<CatechismClassEucharisticYouthDto>> handler,
        CancellationToken cancellationToken)
    {
        List<CatechismClassEucharisticYouthDto> result = await handler.Handle(
            new GetCatechismClassEucharisticYouthsByYouthQuery(youthId), cancellationToken);
        return Ok(result);
    }

    [HttpGet("contacts")]
    public async Task<IActionResult> GetContacts(
        [FromQuery] Guid youthId,
        [FromServices] IRequestHandler<GetEucharisticYouthContactsQuery, List<EucharisticYouthContactDto>> handler,
        CancellationToken cancellationToken)
    {
        List<EucharisticYouthContactDto> result = await handler.Handle(
            new GetEucharisticYouthContactsQuery(youthId), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Link an existing contact to a youth
    /// </summary>
    [HttpPost("contacts")]
    public async Task<IActionResult> LinkContact(
        LinkContactToYouthCommand command,
        [FromServices] IRequestHandler<LinkContactToYouthCommand, Guid?> handler,
        CancellationToken cancellationToken)
    {
        Guid? id = await handler.Handle(command, cancellationToken);

        return id == null ? Forbid() : Ok(id);
    }

    /// <summary>
    /// Unlink a contact from a youth (contact itself is kept)
    /// </summary>
    [HttpDelete("contacts")]
    public async Task<IActionResult> UnlinkContact(
        UnlinkContactFromYouthCommand command,
        [FromServices] IRequestHandler<UnlinkContactFromYouthCommand, bool> handler,
        CancellationToken cancellationToken)
    {
        return await handler.Handle(command, cancellationToken) ? Ok() : Forbid();
    }

    #endregion
}