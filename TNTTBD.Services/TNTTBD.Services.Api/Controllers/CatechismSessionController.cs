using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.CatechismSessions.Commands;
using TNTTBD.Services.Application.CatechismSessions.DTOs;
using TNTTBD.Services.Application.CatechismSessions.Queries;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/sessions")]
public class CatechismSessionController : Controller
{
    #region Fields

    private readonly IMediator _mediator;
    private readonly IAuthorizationContext _authContext;

    #endregion

    #region Constructors

    public CatechismSessionController(IMediator mediator, IAuthorizationContext authContext)
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
    public async Task<IActionResult> Create([FromBody] CreateCatechismSessionCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanAccessClassAsync(command.CatechismClassId, cancellationToken))
        {
            return Forbid();
        }

        Guid id = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>
    /// Get list (paginated, optional classId filter)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetCatechismSessionsQuery query, CancellationToken cancellationToken)
    {
        PaginatedResult<CatechismSessionDto> result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get by id
    /// </summary>
    [HttpGet("detail")]
    public async Task<IActionResult> GetById([FromQuery] GetCatechismSessionByIdQuery query, CancellationToken cancellationToken)
    {
        CatechismSessionDto result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Update
    /// </summary>
    [HttpPatch]
    public async Task<IActionResult> Update(UpdateCatechismSessionCommand command, CancellationToken cancellationToken)
    {
        if (await _mediator.Send(command, cancellationToken))
        {
            return Ok();
        }

        return Forbid();
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(RemoveCatechismSessionCommand command, CancellationToken cancellationToken)
    {
        // Permission to be added

        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpPost("attendances")]
    public async Task<IActionResult> MarkAttendances([FromBody] MarkAttendanceListCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanAccessCatechistAsync(command.MarkedByCatechistId, cancellationToken))
        {
            return Forbid();
        }

        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpPost("attendance")]
    public async Task<IActionResult> MarkAttendance([FromBody] MarkAttendanceCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanAccessCatechistAsync(command.MarkedByCatechistId, cancellationToken))
        {
            return Forbid();
        }

        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpGet("attendances")]
    public async Task<IActionResult> GetAttendances([FromQuery] GetAttendancesBySessionQuery query, CancellationToken cancellationToken)
    {
        PaginatedResult<CatechismSessionAttendanceDto> result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("attendance-sheet")]
    public async Task<IActionResult> GetAttendanceSheet([FromQuery] GetAttendanceSheetQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    #endregion
}