using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.CatechismClassEucharisticYouths.Commands;
using TNTTBD.Services.Application.CatechismClassEucharisticYouths.DTOs;
using TNTTBD.Services.Application.CatechismClassEucharisticYouths.Queries;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/class-youth")]
public class CatechismClassEucharisticYouthController : Controller
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
        [FromBody] AddEucharisticYouthToClassCommand command,
        [FromServices] IRequestHandler<AddEucharisticYouthToClassCommand, Guid?> handler,
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
    /// Get list (paginated, optional classId filter)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetCatechismClassEucharisticYouthsQuery query,
        [FromServices] IRequestHandler<GetCatechismClassEucharisticYouthsQuery, PaginatedResult<CatechismClassEucharisticYouthDto>> handler,
        CancellationToken cancellationToken)
    {
        PaginatedResult<CatechismClassEucharisticYouthDto> result = await handler.Handle(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get by id
    /// </summary>
    [HttpGet("detail")]
    public async Task<IActionResult> GetById(
        [FromQuery] GetCatechismClassEucharisticYouthByIdQuery query,
        [FromServices] IRequestHandler<GetCatechismClassEucharisticYouthByIdQuery, CatechismClassEucharisticYouthDto> handler,
        CancellationToken cancellationToken)
    {
        CatechismClassEucharisticYouthDto result = await handler.Handle(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Update
    /// </summary>
    [HttpPatch]
    public async Task<IActionResult> Update(
        UpdateCatechismClassEucharisticYouthCommand command,
        [FromServices] IRequestHandler<UpdateCatechismClassEucharisticYouthCommand, bool> handler,
        CancellationToken cancellationToken)
    {
        if (await handler.Handle(command, cancellationToken))
        {
            return Ok();
        }
        return Forbid();
    }

    /// <summary>
    /// Update
    /// </summary>
    [HttpPatch("mark-completed")]
    public async Task<IActionResult> MarkCompleted(
        MarkCatechismClassEucharisticYouthsCompletedCommand command,
        [FromServices] IRequestHandler<MarkCatechismClassEucharisticYouthsCompletedCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.Handle(command, cancellationToken);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(
        RemoveEucharisticYouthFromClassCommand command,
        [FromServices] IRequestHandler<RemoveEucharisticYouthFromClassCommand, bool> handler,
        CancellationToken cancellationToken)
    {
        if (await handler.Handle(command, cancellationToken))
        {
            return Ok();
        }
        return Forbid();
    }

    [HttpPost("migrate")]
    public async Task<IActionResult> Migrate(
        [FromBody] MigrateYouthToClassCommand command,
        [FromServices] IRequestHandler<MigrateYouthToClassCommand, int> handler,
        CancellationToken cancellationToken)
    {
        int count = await handler.Handle(command, cancellationToken);
        return count switch
        {
            -1 => Forbid(),
            0 => BadRequest(),
            _ => Ok(new { migrated = count })
        };
    }

    #endregion
}