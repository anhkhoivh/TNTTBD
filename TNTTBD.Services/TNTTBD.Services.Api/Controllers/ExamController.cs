using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.Exams.Commands;
using TNTTBD.Services.Application.Exams.DTOs;
using TNTTBD.Services.Application.Exams.Queries;

namespace TNTTBD.Services.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/exams")]
public class ExamController : Controller
{
    private readonly IMediator _mediator;
    private readonly IAuthorizationContext _authContext;

    public ExamController(IMediator mediator, IAuthorizationContext authContext)
    {
        _mediator = mediator;
        _authContext = authContext;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateExamCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanAccessClassAsync(command.CatechismClassId, cancellationToken))
        {
            return Forbid();
        }

        Guid id = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetExamsQuery query, CancellationToken cancellationToken)
    {
        PaginatedResult<ExamDto> result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("detail")]
    public async Task<IActionResult> GetById([FromQuery] GetExamByIdQuery query, CancellationToken cancellationToken)
    {
        ExamDto result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPatch]
    public async Task<IActionResult> Update([FromBody] UpdateExamCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.IsAdminAsync(cancellationToken))
        {
            return Forbid();
        }

        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Remove([FromBody] RemoveExamCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.IsAdminAsync(cancellationToken))
        {
            return Forbid();
        }

        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpPost("results")]
    public async Task<IActionResult> SaveResults([FromBody] SaveExamResultsCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.IsAdminAsync(cancellationToken))
        {
            return Forbid();
        }

        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpGet("my-results")]
    public async Task<IActionResult> GetMyResults([FromQuery] GetMyExamResultsQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}
