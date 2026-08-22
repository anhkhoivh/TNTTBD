using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    private readonly IMediator _mediator;

    public CatechismClassCatechistController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AssignCatechistToClassCommand command, CancellationToken cancellationToken)
    {
        Guid? id = await _mediator.Send(command, cancellationToken);

        return id switch
        {
            null => Forbid(),
            _ => CreatedAtAction(nameof(GetById), new { id }, id)
        };
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetCatechismClassCatechistsQuery query, CancellationToken cancellationToken)
    {
        PaginatedResult<CatechismClassCatechistDto> result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("detail")]
    public async Task<IActionResult> GetById([FromQuery] GetCatechismClassCatechistByIdQuery query, CancellationToken cancellationToken)
    {
        CatechismClassCatechistDto result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPatch]
    public async Task<IActionResult> Update([FromBody] UpdateCatechistClassAssignmentCommand command, CancellationToken cancellationToken)
    {
        if (await _mediator.Send(command, cancellationToken))
        {
            return Ok();
        }
        return Forbid();
    }

    [HttpDelete]
    public async Task<IActionResult> Remove([FromBody] RemoveCatechistFromClassCommand command, CancellationToken cancellationToken)
    {
        if (await _mediator.Send(command, cancellationToken))
        {
            return Ok();
        }
        return Forbid();
    }
}
