using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TNTTBD.Services.Application.Reports.DTOs;
using TNTTBD.Services.Application.Reports.Queries;

namespace TNTTBD.Services.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/reports")]
public class ReportController(IMediator mediator) : Controller
{
    [HttpGet("classes/{classId:guid}")]
    public async Task<IActionResult> GetClassReport(Guid classId, CancellationToken cancellationToken)
    {
        ClassReportDto? result = await mediator.Send(new GetClassReportQuery(classId), cancellationToken);
        return result == null ? NotFound() : Ok(result);
    }

    [HttpGet("youths/{youthId:guid}")]
    public async Task<IActionResult> GetYouthReport(
        Guid youthId,
        [FromQuery] Guid? catechismYearId,
        [FromQuery] int page = 1,
        [FromQuery] int? pageSize = null,
        CancellationToken cancellationToken = default)
    {
        YouthReportDto? result = await mediator.Send(
            new GetYouthReportQuery(youthId, catechismYearId, page, pageSize),
            cancellationToken);

        return result == null ? NotFound() : Ok(result);
    }
}