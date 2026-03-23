using MediatR;
using Microsoft.AspNetCore.Mvc;
using TNTTBD.Services.Application.Catechists.Commands;
using TNTTBD.Services.Application.Catechists.DTOs;
using TNTTBD.Services.Application.Catechists.Queries;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Api.Controllers;

[ApiController]
[Route("api/catechists")]
public class CatechistController : Controller
{
    #region Fields

    private readonly IMediator _mediator;

    #endregion

    #region Constructors

    public CatechistController(IMediator mediator)
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
    public async Task<IActionResult> Create([FromBody] CreateCatechistCommand command, CancellationToken cancellationToken)
    {
        Guid id = await _mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>
    /// Get by id
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetById(GetCatechistByIdQuery query, CancellationToken cancellationToken)
    {
        CatechistDto? result = await _mediator.Send(query, cancellationToken);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    /// <summary>
    /// Update
    /// </summary>
    [HttpPatch]
    public async Task<IActionResult> Update(UpdateCatechistCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(RemoveCatechistCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Add Patron Saints
    /// </summary>
    [HttpPost("patronSaints")]
    public async Task<IActionResult> AddPatronSaint(AddCatechistPatronSaintCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    /// <summary>
    /// Add Patron Saints
    /// </summary>
    [HttpDelete("patronSaints")]
    public async Task<IActionResult> RemovePatronSaint(RemoveCatechistPatronSaintCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpPost("photos/upload-url")]
    public async Task<IActionResult> RequestUploadUrl(RequestCatechistPhotoUploadCommand command, CancellationToken cancellationToken)
    {
        PhotoReferenceDto result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }

    [HttpGet("photos")]
    public async Task<IActionResult> GetPhotos(GetCatechistPhotosQuery query, CancellationToken cancellationToken)
    {
        List<PhotoReferenceDto> result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("photos")]
    public async Task<IActionResult> RemovePhoto(RemoveCatechistPhotoCommand command, CancellationToken cancellationToken)
    {
        await _mediator.Send(command, cancellationToken);
        return Ok();
    }

    #endregion
}