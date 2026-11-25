using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TNTTBD.Services.Application.Abstractions;
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
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetContactsQuery query,
        [FromServices] IRequestHandler<GetContactsQuery, PaginatedResult<ContactDto>> handler,
        CancellationToken cancellationToken)
    {
        PaginatedResult<ContactDto> result = await handler.Handle(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("detail")]
    public async Task<IActionResult> GetById(
        [FromQuery] GetContactByIdQuery query,
        [FromServices] IRequestHandler<GetContactByIdQuery, ContactDetailDto?> handler,
        CancellationToken cancellationToken)
    {
        ContactDetailDto? result = await handler.Handle(query, cancellationToken);

        return result == null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateContactCommand command,
        [FromServices] IRequestHandler<CreateContactCommand, Guid?> handler,
        CancellationToken cancellationToken)
    {
        Guid? id = await handler.Handle(command, cancellationToken);

        return id switch
        {
            null => Forbid(),
            _ => CreatedAtAction(nameof(GetById), new { id }, id)
        };
    }

    [HttpPatch]
    public async Task<IActionResult> Update(
        UpdateContactCommand command,
        [FromServices] IRequestHandler<UpdateContactCommand, bool> handler,
        CancellationToken cancellationToken)
    {
        return await handler.Handle(command, cancellationToken) ? Ok() : Forbid();
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(
        DeleteContactCommand command,
        [FromServices] IRequestHandler<DeleteContactCommand, bool> handler,
        CancellationToken cancellationToken)
    {
        return await handler.Handle(command, cancellationToken) ? Ok() : Forbid();
    }
}
