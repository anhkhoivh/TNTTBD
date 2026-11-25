using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.CatechismClassCatechists.Commands;
using TNTTBD.Services.Application.CatechismClassCatechists.DTOs;
using TNTTBD.Services.Application.CatechismClassCatechists.Queries;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/class-catechist")]
public class CatechismClassCatechistController : Controller
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] AssignCatechistToClassCommand command,
        [FromServices] IRequestHandler<AssignCatechistToClassCommand, Guid?> handler,
        CancellationToken cancellationToken)
    {
        Guid? id = await handler.Handle(command, cancellationToken);

        return id switch
        {
            null => Forbid(),
            _ => CreatedAtAction(nameof(GetById), new { id }, id)
        };
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetCatechismClassCatechistsQuery query,
        [FromServices] IRequestHandler<GetCatechismClassCatechistsQuery, PaginatedResult<CatechismClassCatechistDto>> handler,
        CancellationToken cancellationToken)
    {
        PaginatedResult<CatechismClassCatechistDto> result = await handler.Handle(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("detail")]
    public async Task<IActionResult> GetById(
        [FromQuery] GetCatechismClassCatechistByIdQuery query,
        [FromServices] IRequestHandler<GetCatechismClassCatechistByIdQuery, CatechismClassCatechistDto> handler,
        CancellationToken cancellationToken)
    {
        CatechismClassCatechistDto result = await handler.Handle(query, cancellationToken);
        return Ok(result);
    }

    [HttpPatch]
    public async Task<IActionResult> Update(
        [FromBody] UpdateCatechistClassAssignmentCommand command,
        [FromServices] IRequestHandler<UpdateCatechistClassAssignmentCommand, bool> handler,
        CancellationToken cancellationToken)
    {
        if (await handler.Handle(command, cancellationToken))
        {
            return Ok();
        }
        return Forbid();
    }

    [HttpPatch("status")]
    public async Task<IActionResult> UpdateStatus(
        [FromBody] UpdateCatechistClassStatusCommand command,
        [FromServices] IRequestHandler<UpdateCatechistClassStatusCommand, bool> handler,
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
        [FromBody] RemoveCatechistFromClassCommand command,
        [FromServices] IRequestHandler<RemoveCatechistFromClassCommand, bool> handler,
        CancellationToken cancellationToken)
    {
        if (await handler.Handle(command, cancellationToken))
        {
            return Ok();
        }
        return Forbid();
    }
}
