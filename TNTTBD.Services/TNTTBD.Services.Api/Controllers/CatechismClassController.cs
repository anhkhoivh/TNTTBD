using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    #region Fields

    private readonly IMediator _mediator;
    private readonly IAuthorizationContext _authContext;

    #endregion

    #region Constructors

    public CatechismClassController(IMediator mediator, IAuthorizationContext authContext)
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
    public async Task<IActionResult> Create([FromBody] CreateCatechismClassCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.IsAdminAsync(cancellationToken))
        {
            return Forbid();
        }

        Guid id = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>
    /// Get list (paginated, optional yearId filter)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetCatechismClassesQuery query, CancellationToken cancellationToken)
    {
        PaginatedResult<CatechismClassDto> result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get by id
    /// </summary>
    [HttpGet("detail")]
    public async Task<IActionResult> GetById([FromQuery] GetCatechismClassByIdQuery query, CancellationToken cancellationToken)
    {
        CatechismClassDto result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get sessions of class
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("sessions")]
    public async Task<IActionResult> GetSessionsByClass([FromBody] GetSessionsByClassQuery query, CancellationToken cancellationToken)
    {
        List<CatechismSessionDto> classes = await _mediator.Send(query, cancellationToken);
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
    public async Task<IActionResult> Update(UpdateCatechismClassCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanAccessClassAsync(command.CatechismClassId, cancellationToken))
        {
            return Forbid();
        }

        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(RemoveCatechismClassCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.IsAdminAsync(cancellationToken))
        {
            return Forbid();
        }

        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    #endregion
}