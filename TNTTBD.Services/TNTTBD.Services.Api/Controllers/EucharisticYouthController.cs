using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
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

    private readonly IMediator _mediator;
    private readonly IAuthorizationContext _authContext;
    private readonly IFileStorage _storage;
    private readonly ILogger<EucharisticYouthController> _logger;

    #endregion

    #region Constructors

    public EucharisticYouthController(IMediator mediator, IAuthorizationContext authContext, IFileStorage storage, ILogger<EucharisticYouthController> logger)
    {
        _mediator = mediator;
        _authContext = authContext;
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
    public async Task<IActionResult> Create([FromBody] CreateEucharisticYouthCommand command, CancellationToken cancellationToken)
    {
        Guid? id = await _mediator.Send(command, cancellationToken);

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
    public async Task<IActionResult> GetAll([FromQuery] GetEucharisticYouthsQuery query, CancellationToken cancellationToken)
    {
        PaginatedResult<EucharisticYouthDto> result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get by id
    /// </summary>
    [HttpGet("detail")]
    public async Task<IActionResult> GetById([FromQuery] GetEucharisticYouthByIdQuery query, CancellationToken cancellationToken)
    {
        EucharisticYouthDto? result = await _mediator.Send(query, cancellationToken);

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
    public async Task<IActionResult> Update(UpdateEucharisticYouthCommand command, CancellationToken cancellationToken)
    {
        return await _mediator.Send(command, cancellationToken) ? Ok() : Forbid();
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(RemoveEucharisticYouthCommand command, CancellationToken cancellationToken)
    {
        return await _mediator.Send(command, cancellationToken) ? Ok() : Forbid();
    }

    /// <summary>
    /// Add milestone
    /// </summary>
    [HttpPost("milestones")]
    public async Task<IActionResult> AddMilestone(AddEucharisticYouthMilestoneCommand command, CancellationToken cancellationToken)
    {
        return await _mediator.Send(command, cancellationToken) ? Ok() : Forbid();
    }

    /// <summary>
    /// Add milestone
    /// </summary>
    [HttpPut("milestones")]
    public async Task<IActionResult> UpdateMilestone(UpdateEucharisticYouthMilestoneCommand command, CancellationToken cancellationToken)
    {
        return await _mediator.Send(command, cancellationToken) ? Ok() : Forbid();
    }

    [HttpDelete("milestones")]
    public async Task<IActionResult> RemoveMilestone(RemoveEucharisticYouthMilestoneCommand command, CancellationToken cancellationToken)
    {
        return await _mediator.Send(command, cancellationToken) ? Ok() : Forbid();
    }

    /// <summary>
    /// Add Patron Saints
    /// </summary>
    [HttpPost("patronSaints")]
    public async Task<IActionResult> AddPatronSaint(AddEucharisticYouthPatronSaintCommand command, CancellationToken cancellationToken)
    {
        return await _mediator.Send(command, cancellationToken) ? Ok() : Forbid();
    }

    /// <summary>
    /// Add Patron Saints
    /// </summary>
    [HttpDelete("patronSaints")]
    public async Task<IActionResult> RemovePatronSaint(RemoveEucharisticYouthPatronSaintCommand command, CancellationToken cancellationToken)
    {
        return await _mediator.Send(command, cancellationToken) ? Ok() : Forbid();
    }

    /// <summary>
    /// Request photo upload
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("photos/upload-url")]
    public async Task<IActionResult> RequestUploadUrl(RequestEucharisticYouthPhotoUploadCommand command, CancellationToken cancellationToken)
    {
        PhotoReferenceDto? result = await _mediator.Send(command, cancellationToken);

        return result == null ? Forbid() : Ok(result);
    }

    /// <summary>
    /// Get list of photos
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("photos")]
    public async Task<IActionResult> GetPhotos(GetEucharisticYouthPhotosQuery query, CancellationToken cancellationToken)
    {
        List<PhotoReferenceDto> result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get the current user's photo management permission
    /// </summary>
    [HttpGet("photos/permissions")]
    public async Task<IActionResult> GetPhotoPermissions([FromQuery] Guid youthId, CancellationToken cancellationToken)
    {
        return Ok(new { canManage = await _authContext.CanManageYouthPhotosAsync(youthId, cancellationToken) });
    }

    /// <summary>
    /// Update photo purpose
    /// </summary>
    [HttpPatch("photos")]
    public async Task<IActionResult> UpdatePhoto(UpdateEucharisticYouthPhotoCommand command, CancellationToken cancellationToken)
    {
        return await _mediator.Send(command, cancellationToken) ? Ok() : Forbid();
    }

    /// <summary>
    /// Remove photo
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("photos")]
    public async Task<IActionResult> RemovePhoto(RemoveEucharisticYouthPhotoCommand command, CancellationToken cancellationToken)
    {
        string? storageKey = await _mediator.Send(command, cancellationToken);
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
    public async Task<IActionResult> GetEnrollments([FromQuery] Guid youthId, CancellationToken cancellationToken)
    {
        List<CatechismClassEucharisticYouthDto> result = await _mediator.Send(
            new GetCatechismClassEucharisticYouthsByYouthQuery(youthId), cancellationToken);
        return Ok(result);
    }

    [HttpGet("contacts")]
    public async Task<IActionResult> GetContacts([FromQuery] Guid youthId, CancellationToken cancellationToken)
    {
        List<EucharisticYouthContactDto> result = await _mediator.Send(
            new GetEucharisticYouthContactsQuery(youthId), cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Link an existing contact to a youth
    /// </summary>
    [HttpPost("contacts")]
    public async Task<IActionResult> LinkContact(LinkContactToYouthCommand command, CancellationToken cancellationToken)
    {
        Guid? id = await _mediator.Send(command, cancellationToken);

        return id == null ? Forbid() : Ok(id);
    }

    /// <summary>
    /// Unlink a contact from a youth (contact itself is kept)
    /// </summary>
    [HttpDelete("contacts")]
    public async Task<IActionResult> UnlinkContact(UnlinkContactFromYouthCommand command, CancellationToken cancellationToken)
    {
        return await _mediator.Send(command, cancellationToken) ? Ok() : Forbid();
    }

    #endregion
}