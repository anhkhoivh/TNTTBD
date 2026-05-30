using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.CatechismClassEucharisticYouths.DTOs;
using TNTTBD.Services.Application.CatechismClassEucharisticYouths.Queries;
using TNTTBD.Services.Application.Common.DTOs;
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

    #endregion

    #region Constructors

    public EucharisticYouthController(IMediator mediator, IAuthorizationContext authContext)
    {
        _mediator = mediator;
        _authContext = authContext;
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
        if (!await _authContext.IsAdminAsync(cancellationToken))
        {
            return Forbid();
        }

        Guid id = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id }, id);
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
        if (!await _authContext.CanAccessYouthAsync(command.EucharisticYouthId, cancellationToken))
        {
            return Forbid();
        }

        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(RemoveEucharisticYouthCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.IsAdminAsync(cancellationToken))
        {
            return Forbid();
        }

        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Add milestone
    /// </summary>
    [HttpPost("milestones")]
    public async Task<IActionResult> AddMilestone(AddEucharisticYouthMilestoneCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Add milestone
    /// </summary>
    [HttpPut("milestones")]
    public async Task<IActionResult> UpdateMilestone(UpdateEucharisticYouthMilestoneCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpDelete("milestones")]
    public async Task<IActionResult> RemoveMilestone(RemoveEucharisticYouthMilestoneCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Add Patron Saints
    /// </summary>
    [HttpPost("patronSaints")]
    public async Task<IActionResult> AddPatronSaint(AddEucharisticYouthPatronSaintCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Add Patron Saints
    /// </summary>
    [HttpDelete("patronSaints")]
    public async Task<IActionResult> RemovePatronSaint(RemoveEucharisticYouthPatronSaintCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return Ok();
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
        PhotoReferenceDto result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
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
    /// Remove photo
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpDelete("photos")]
    public async Task<IActionResult> RemovePhoto(RemoveEucharisticYouthPhotoCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpGet("enrollments")]
    public async Task<IActionResult> GetEnrollments([FromQuery] Guid youthId, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanAccessYouthAsync(youthId, cancellationToken))
        {
            return Forbid();
        }

        List<CatechismClassEucharisticYouthDto> result = await _mediator.Send(
            new GetCatechismClassEucharisticYouthsByYouthQuery(youthId), cancellationToken);
        return Ok(result);
    }

    [HttpGet("contacts")]
    public async Task<IActionResult> GetContacts([FromQuery] Guid youthId, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanAccessYouthAsync(youthId, cancellationToken))
        {
            return Forbid();
        }

        List<EucharisticYouthContactDto> result = await _mediator.Send(
            new GetEucharisticYouthContactsQuery(youthId), cancellationToken);
        return Ok(result);
    }

    #endregion
}