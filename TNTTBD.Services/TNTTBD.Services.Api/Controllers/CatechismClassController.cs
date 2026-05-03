using MediatR;
using Microsoft.AspNetCore.Mvc;
using TNTTBD.Services.Application.CatechismClasses.Commands;
using TNTTBD.Services.Application.CatechismClasses.DTOs;
using TNTTBD.Services.Application.CatechismClasses.Queries;
using TNTTBD.Services.Application.CatechismSessions.DTOs;
using TNTTBD.Services.Application.CatechismSessions.Queries;

namespace TNTTBD.Services.Api.Controllers;

[ApiController]
[Route("api/classes")]
public class CatechismClassController : Controller
{
    #region Fields

    private readonly IMediator _mediator;

    #endregion

    #region Constructors

    public CatechismClassController(IMediator mediator)
    {
        _mediator = mediator;
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
        Guid id = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>
    /// Get by id
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetById(GetCatechismClassByIdQuery query, CancellationToken cancellationToken)
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
    [HttpPost("classes")]
    public async Task<IActionResult> GetClassesByYear([FromBody] GetSessionsByClassQuery query, CancellationToken cancellationToken)
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
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(RemoveCatechismClassCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    #endregion
}