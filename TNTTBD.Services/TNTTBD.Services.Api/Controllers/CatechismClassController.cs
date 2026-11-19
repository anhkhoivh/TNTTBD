using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.CatechismClasses.Commands;
using TNTTBD.Services.Application.CatechismClasses.DTOs;
using TNTTBD.Services.Application.CatechismClasses.Queries;
using TNTTBD.Services.Application.CatechismSessions.DTOs;
using TNTTBD.Services.Application.CatechismSessions.Queries;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/classes")]
public class CatechismClassController : Controller
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
        [FromBody] CreateCatechismClassCommand command,
        [FromServices] IRequestHandler<CreateCatechismClassCommand, Guid?> handler,
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
    /// Get list (paginated, optional yearId filter)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetCatechismClassesQuery query,
        [FromServices] IRequestHandler<GetCatechismClassesQuery, PaginatedResult<CatechismClassDto>> handler,
        CancellationToken cancellationToken)
    {
        PaginatedResult<CatechismClassDto> result = await handler.Handle(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get by id
    /// </summary>
    [HttpGet("detail")]
    public async Task<IActionResult> GetById(
        [FromQuery] GetCatechismClassByIdQuery query,
        [FromServices] IRequestHandler<GetCatechismClassByIdQuery, CatechismClassDto> handler,
        CancellationToken cancellationToken)
    {
        CatechismClassDto result = await handler.Handle(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get sessions of class
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("sessions")]
    public async Task<IActionResult> GetSessionsByClass(
        [FromBody] GetSessionsByClassQuery query,
        [FromServices] IRequestHandler<GetSessionsByClassQuery, List<CatechismSessionDto>> handler,
        CancellationToken cancellationToken)
    {
        List<CatechismSessionDto> classes = await handler.Handle(query, cancellationToken);
        if (classes == null || classes.Count == 0)
        {
            return NotFound();
        }

        return Ok(classes);
    }

    /// <summary>
    /// Update
    /// </summary>
    [HttpPatch]
    public async Task<IActionResult> Update(
        UpdateCatechismClassCommand command,
        [FromServices] IRequestHandler<UpdateCatechismClassCommand, bool> handler,
        CancellationToken cancellationToken)
    {
        await handler.Handle(command, cancellationToken);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(
        RemoveCatechismClassCommand command,
        [FromServices] IRequestHandler<RemoveCatechismClassCommand, bool> handler,
        CancellationToken cancellationToken)
    {
        if (await handler.Handle(command, cancellationToken))
        {
            return Ok();
        }

        return Forbid();
    }

    #endregion
}