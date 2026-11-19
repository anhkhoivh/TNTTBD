using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TNTTBD.Services.Application.Abstractions;
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
    #region Methods

    /// <summary>
    /// Create
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateCatechismSessionCommand command,
        [FromServices] IRequestHandler<CreateCatechismSessionCommand, Guid> handler,
        CancellationToken cancellationToken)
    {
        Guid id = await handler.Handle(command, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>
    /// Get list (paginated, optional classId filter)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetCatechismSessionsQuery query,
        [FromServices] IRequestHandler<GetCatechismSessionsQuery, PaginatedResult<CatechismSessionDto>> handler,
        CancellationToken cancellationToken)
    {
        PaginatedResult<CatechismSessionDto> result = await handler.Handle(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get by id
    /// </summary>
    [HttpGet("detail")]
    public async Task<IActionResult> GetById(
        [FromQuery] GetCatechismSessionByIdQuery query,
        [FromServices] IRequestHandler<GetCatechismSessionByIdQuery, CatechismSessionDto> handler,
        CancellationToken cancellationToken)
    {
        CatechismSessionDto result = await handler.Handle(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Update
    /// </summary>
    [HttpPatch]
    public async Task<IActionResult> Update(
        UpdateCatechismSessionCommand command,
        [FromServices] IRequestHandler<UpdateCatechismSessionCommand, bool> handler,
        CancellationToken cancellationToken)
    {
        if (await handler.Handle(command, cancellationToken))
        {
            return Ok();
        }

        return Forbid();
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(
        RemoveCatechismSessionCommand command,
        [FromServices] IRequestHandler<RemoveCatechismSessionCommand> handler,
        CancellationToken cancellationToken)
    {
        // Permission to be added

        await handler.Handle(command, cancellationToken);
        return Ok();
    }

    [HttpPost("attendances")]
    public async Task<IActionResult> MarkAttendances(
        [FromBody] MarkAttendanceListCommand command,
        [FromServices] IRequestHandler<MarkAttendanceListCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.Handle(command, cancellationToken);
        return Ok();
    }

    [HttpPost("attendance")]
    public async Task<IActionResult> MarkAttendance(
        [FromBody] MarkAttendanceCommand command,
        [FromServices] IRequestHandler<MarkAttendanceCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.Handle(command, cancellationToken);
        return Ok();
    }

    [HttpGet("attendances")]
    public async Task<IActionResult> GetAttendances(
        [FromQuery] GetAttendancesBySessionQuery query,
        [FromServices] IRequestHandler<GetAttendancesBySessionQuery, PaginatedResult<CatechismSessionAttendanceDto>> handler,
        CancellationToken cancellationToken)
    {
        PaginatedResult<CatechismSessionAttendanceDto> result = await handler.Handle(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("attendance-sheet")]
    public async Task<IActionResult> GetAttendanceSheet(
        [FromQuery] GetAttendanceSheetQuery query,
        [FromServices] IRequestHandler<GetAttendanceSheetQuery, List<CatechismSessionAttendanceDto>> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(query, cancellationToken);
        return Ok(result);
    }

    #endregion
}