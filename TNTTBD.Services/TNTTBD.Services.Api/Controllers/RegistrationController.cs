using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Registration.Commands;

namespace TNTTBD.Services.Api.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/registration")]
public class RegistrationController : Controller
{
    [HttpPost("youth")]
    public async Task<IActionResult> RegisterYouth(
        [FromBody] RegisterEucharisticYouthCommand command,
        [FromServices] IRequestHandler<RegisterEucharisticYouthCommand, Guid> handler,
        CancellationToken cancellationToken)
    {
        Guid id = await handler.Handle(command, cancellationToken);
        return Created($"/api/youths/detail?id={id}", id);
    }
}
