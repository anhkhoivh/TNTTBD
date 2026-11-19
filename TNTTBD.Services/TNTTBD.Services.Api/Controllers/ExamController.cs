using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TNTTBD.Services.Application.Abstractions;
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
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateExamCommand command,
        [FromServices] IRequestHandler<CreateExamCommand, Guid> handler,
        CancellationToken cancellationToken)
    {
        Guid id = await handler.Handle(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetExamsQuery query,
        [FromServices] IRequestHandler<GetExamsQuery, PaginatedResult<ExamDto>> handler,
        CancellationToken cancellationToken)
    {
        PaginatedResult<ExamDto> result = await handler.Handle(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("detail")]
    public async Task<IActionResult> GetById(
        [FromQuery] GetExamByIdQuery query,
        [FromServices] IRequestHandler<GetExamByIdQuery, ExamDto> handler,
        CancellationToken cancellationToken)
    {
        ExamDto result = await handler.Handle(query, cancellationToken);
        return Ok(result);
    }

    [HttpPatch]
    public async Task<IActionResult> Update(
        [FromBody] UpdateExamCommand command,
        [FromServices] IRequestHandler<UpdateExamCommand> handler,
        CancellationToken cancellationToken)
    {
        // Permission to be added
        await handler.Handle(command, cancellationToken);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(
        [FromBody] RemoveExamCommand command,
        [FromServices] IRequestHandler<RemoveExamCommand> handler,
        CancellationToken cancellationToken)
    {
        // Permission to be added

        await handler.Handle(command, cancellationToken);
        return Ok();
    }

    [HttpPost("results")]
    public async Task<IActionResult> SaveResults(
        [FromBody] SaveExamResultsCommand command,
        [FromServices] IRequestHandler<SaveExamResultsCommand, bool> handler,
        CancellationToken cancellationToken)
    {
        if (await handler.Handle(command, cancellationToken))
        {
            return Ok();
        }
        return Forbid();
    }

    [HttpGet("my-results")]
    public async Task<IActionResult> GetMyResults(
        [FromQuery] GetMyExamResultsQuery query,
        [FromServices] IRequestHandler<GetMyExamResultsQuery, PaginatedResult<MyExamResultDto>> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(query, cancellationToken);
        return Ok(result);
    }
}
