using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TNTTBD.Services.Application.Abstractions;
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
    #region Methods

    /// <summary>
    /// Create
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateCatechismYearCommand command,
        [FromServices] IRequestHandler<CreateCatechismYearCommand, Guid> handler,
        CancellationToken cancellationToken)
    {
        Guid id = await handler.Handle(command, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>
    /// Get list (paginated)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetCatechismYearsQuery query,
        [FromServices] IRequestHandler<GetCatechismYearsQuery, PaginatedResult<CatechismYearDto>> handler,
        CancellationToken cancellationToken)
    {
        PaginatedResult<CatechismYearDto> result = await handler.Handle(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get by id
    /// </summary>
    [HttpGet("detail")]
    public async Task<IActionResult> GetById(
        [FromQuery] GetCatechismYearByIdQuery query,
        [FromServices] IRequestHandler<GetCatechismYearByIdQuery, CatechismYearDto> handler,
        CancellationToken cancellationToken)
    {
        CatechismYearDto result = await handler.Handle(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Get classes by year
    /// </summary>
    [HttpGet("classes")]
    public async Task<IActionResult> GetClassesByYear(
        [FromQuery] GetClassesByYearQuery query,
        [FromServices] IRequestHandler<GetClassesByYearQuery, List<CatechismClassDto>> handler,
        CancellationToken cancellationToken)
    {
        List<CatechismClassDto> classes = await handler.Handle(query, cancellationToken);
        return Ok(classes ?? []);
    }

    /// <summary>
    /// Update
    /// </summary>
    [HttpPatch]
    public async Task<IActionResult> Update(
        UpdateCatechismYearCommand command,
        [FromServices] IRequestHandler<UpdateCatechismYearCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.Handle(command, cancellationToken);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(
        RemoveCatechismYearCommand command,
        [FromServices] IRequestHandler<RemoveCatechismYearCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.Handle(command, cancellationToken);
        return Ok();
    }

    #endregion
}