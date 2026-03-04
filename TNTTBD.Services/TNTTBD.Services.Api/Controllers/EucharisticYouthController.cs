using MediatR;
using Microsoft.AspNetCore.Mvc;
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
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        EucharisticYouthDto? result = await _mediator.Send(new GetEucharisticYouthByIdQuery(id), cancellationToken);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    /// <summary>
    /// Add milestone
    /// </summary>
    [HttpPost("{id}/milestones")]
    public async Task<IActionResult> AddMilestone(Guid id, AddMilestoneCommand command, CancellationToken cancellationToken)
    {
        AddMilestoneCommand finalCommand = command with { EucharisticYouthId = id };
        await _mediator.Send(finalCommand, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Add photo
    /// </summary>
    [HttpPost("{id}/photos")]
    public async Task<IActionResult> AddPhoto(Guid id, AddPhotoCommand command, CancellationToken cancellationToken)
    {
        AddPhotoCommand finalCommand = command with { EucharisticYouthId = id };
        await _mediator.Send(finalCommand, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Add Patron Saints
    /// </summary>
    [HttpPost("{id}/patronSaints")]
    public async Task<IActionResult> AddPatronSaint(Guid id, AddPatronSaintCommand command, CancellationToken cancellationToken)
    {
        AddPatronSaintCommand finalCommand = command with { EucharisticYouthId = id };
        await _mediator.Send(finalCommand, cancellationToken);
        return Ok();
    }

    #endregion
}