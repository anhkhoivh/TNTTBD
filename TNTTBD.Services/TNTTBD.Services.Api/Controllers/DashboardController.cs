using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Dashboard.DTOs;
using TNTTBD.Services.Application.Dashboard.Queries;

namespace TNTTBD.Services.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/dashboard")]
public class DashboardController : Controller
{
    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary(
        [FromServices] IRequestHandler<GetDashboardSummaryQuery, DashboardSummaryDto?> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new GetDashboardSummaryQuery(), cancellationToken);
        if (result == null)
        {
            return Unauthorized();
        }

        return Ok(result);
    }
}
