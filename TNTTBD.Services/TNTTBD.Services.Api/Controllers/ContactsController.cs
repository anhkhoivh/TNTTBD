using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.Contacts.Commands;
using TNTTBD.Services.Application.Contacts.DTOs;
using TNTTBD.Services.Application.Contacts.Queries;

namespace TNTTBD.Services.Api.Controllers;

[ApiController]
[Route("api/contacts")]
[Authorize]
public class ContactsController : Controller
{
    private readonly IMediator _mediator;

    public ContactsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetContactsQuery query, CancellationToken cancellationToken)
    {
        PaginatedResult<ContactDto> result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("detail")]
    public async Task<IActionResult> GetById([FromQuery] GetContactByIdQuery query, CancellationToken cancellationToken)
    {
        ContactDetailDto? result = await _mediator.Send(query, cancellationToken);

        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateContactCommand command, CancellationToken cancellationToken)
    {
        Guid? id = await _mediator.Send(command, cancellationToken);

        return id switch
        {
            null => Forbid(),
            _ => CreatedAtAction(nameof(GetById), new { id }, id)
        };
    }

    [HttpPatch]
    public async Task<IActionResult> Update(UpdateContactCommand command, CancellationToken cancellationToken)
    {
        return await _mediator.Send(command, cancellationToken) ? Ok() : Forbid();
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(DeleteContactCommand command, CancellationToken cancellationToken)
    {
        return await _mediator.Send(command, cancellationToken) ? Ok() : Forbid();
    }
}
