using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TNTTBD.Services.Application.Registration.Commands;

namespace TNTTBD.Services.Api.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/registration")]
public class RegistrationController : Controller
{
    private readonly IMediator _mediator;

    public RegistrationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("youth")]
    public async Task<IActionResult> RegisterYouth([FromBody] RegisterEucharisticYouthCommand command, CancellationToken cancellationToken)
    {
        Guid id = await _mediator.Send(command, cancellationToken);
        return Created($"/api/youths/detail?id={id}", id);
    }
}
