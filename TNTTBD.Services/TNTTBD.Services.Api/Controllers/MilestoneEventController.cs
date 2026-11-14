using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TNTTBD.Services.Application.MilestoneEvents.Commands;
using TNTTBD.Services.Application.MilestoneEvents.DTOs;
using TNTTBD.Services.Application.MilestoneEvents.Queries;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Api.Controllers;

/// <summary>
/// Group sacrament events. Signed-in users can view; Admins and Catechists edit; Admins delete.
/// </summary>
[ApiController]
[Authorize]
[Route("api/milestone-events")]
public class MilestoneEventController(IMediator mediator) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        [FromQuery] MilestoneType? type,
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to,
        [FromQuery] int page = 1,
        [FromQuery] int? pageSize = null,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(new GetMilestoneEventsQuery(search, type, from, to, page, pageSize), cancellationToken));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        MilestoneEventDetailDto? result = await mediator.Send(new GetMilestoneEventQuery(id), cancellationToken);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Save(SaveMilestoneEventCommand command, CancellationToken cancellationToken)
    {
        Guid id = await mediator.Send(command, cancellationToken);
        return Ok(new { id });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteMilestoneEventCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpGet("candidates")]
    public async Task<IActionResult> GetCandidates(
        [FromQuery] MilestoneType type,
        [FromQuery] string? search,
        [FromQuery] Guid? classId,
        [FromQuery] Guid? catechismYearId,
        [FromQuery] DateOnly? date,
        [FromQuery] int page = 1,
        [FromQuery] int? pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        return Ok(await mediator.Send(new GetMilestoneEventCandidatesQuery(type, search, classId, catechismYearId, date, page, pageSize), cancellationToken));
    }

    [HttpPost("generate-codes")]
    public async Task<IActionResult> GenerateCodes(GenerateMilestoneCodesQuery query, CancellationToken cancellationToken)
    {
        return Ok(await mediator.Send(query, cancellationToken));
    }

    [HttpGet("{id:guid}/excel")]
    public async Task<IActionResult> ExportExcel(Guid id, [FromQuery] string language = "vi", CancellationToken cancellationToken = default)
    {
        MilestoneEventExcelFileDto? result = await mediator.Send(new ExportMilestoneEventExcelQuery(id, language), cancellationToken);
        return result == null
            ? NotFound()
            : File(result.Content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.FileName);
    }
}
