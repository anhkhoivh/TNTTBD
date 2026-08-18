using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.Contacts.DTOs;
using TNTTBD.Services.Application.Contacts.Queries;

namespace TNTTBD.Services.Api.Controllers;

[ApiController]
[Route("api/contacts")]
[Authorize(Roles = "Admin")]
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
}
