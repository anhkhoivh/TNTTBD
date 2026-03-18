using MediatR;
using Microsoft.AspNetCore.Mvc;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.EucharisticYouths.Commands;
using TNTTBD.Services.Application.EucharisticYouths.DTOs;
using TNTTBD.Services.Application.EucharisticYouths.Queries;

namespace TNTTBD.Services.Api.Controllers;

[ApiController]
[Route("api/youths")]
public class EucharisticYouthController : Controller
{
    #region Fields

    private readonly IMediator _mediator;

    #endregion

    #region Constructors

    public EucharisticYouthController(IMediator mediator)
    {
        _mediator = mediator;
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
        Guid id = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>
    /// Get by id
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetById(GetEucharisticYouthByIdQuery query, CancellationToken cancellationToken)
    {
        EucharisticYouthDto? result = await _mediator.Send(query, cancellationToken);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    /// <summary>
    /// Add milestone
    /// </summary>
    [HttpPost("milestones")]
    public async Task<IActionResult> AddMilestone(AddMilestoneCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Add milestone
    /// </summary>
    [HttpPut("milestones")]
    public async Task<IActionResult> UpdateMilestone(Guid id, AddMilestoneCommand command, CancellationToken cancellationToken)
    {
        AddMilestoneCommand finalCommand = command with { EucharisticYouthId = id };
        await _mediator.Send(finalCommand, cancellationToken);
        return Ok();
    }

    [HttpDelete("milestones")]
    public async Task<IActionResult> RemoveMilestone(RemoveMilestoneCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Add Patron Saints
    /// </summary>
    [HttpPost("patronSaints")]
    public async Task<IActionResult> AddPatronSaint(AddPatronSaintCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Add Patron Saints
    /// </summary>
    [HttpDelete("patronSaints")]
    public async Task<IActionResult> RemovePatronSaint(RemovePatronSaintCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpPost("photos/upload-url")]
    public async Task<IActionResult> RequestUploadUrl(RequestYouthPhotoUploadCommand command, CancellationToken cancellationToken)
    {
        PhotoReferenceDto result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    [HttpGet("photos")]
    public async Task<IActionResult> GetPhotos(GetYouthPhotosQuery query, CancellationToken cancellationToken)
    {
        List<PhotoReferenceDto> result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("photos")]
    public async Task<IActionResult> RemoveYouthPhoto(RemoveYouthPhotoCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    #endregion
}