using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TNTTBD.Services.Application.CatechismClasses.DTOs;
using TNTTBD.Services.Application.CatechismClasses.Queries;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.CatechismYears.Commands;
using TNTTBD.Services.Application.CatechismYears.DTOs;
using TNTTBD.Services.Application.CatechismYears.Queries;

namespace TNTTBD.Services.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/catechism-years")]
public class CatechismYearController : Controller
{
    #region Fields

    private readonly IMediator _mediator;

    #endregion

    #region Constructors

    public CatechismYearController(IMediator mediator)
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
    public async Task<IActionResult> Create([FromBody] CreateCatechismYearCommand command, CancellationToken cancellationToken)
    {
        Guid id = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>
    /// Get list (paginated)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetCatechismYearsQuery query, CancellationToken cancellationToken)
    {
        PaginatedResult<CatechismYearDto> result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get by id
    /// </summary>
    [HttpGet("detail")]
    public async Task<IActionResult> GetById([FromQuery] GetCatechismYearByIdQuery query, CancellationToken cancellationToken)
    {
        CatechismYearDto result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get classes by year
    /// </summary>
    [HttpGet("classes")]
    public async Task<IActionResult> GetClassesByYear([FromQuery] GetClassesByYearQuery query, CancellationToken cancellationToken)
    {
        List<CatechismClassDto> classes = await _mediator.Send(query, cancellationToken);
        return Ok(classes ?? []);
    }

    /// <summary>
    /// Update
    /// </summary>
    [HttpPatch]
    public async Task<IActionResult> Update(UpdateCatechismYearCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(RemoveCatechismYearCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    #endregion
}