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

    [HttpPost("classes/{classId:guid}/excel")]
    public async Task<IActionResult> ExportClassReportExcel(
        Guid classId,
        [FromBody] ClassReportExcelRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new ExportClassReportExcelQuery(
                classId,
                request.AttendanceEnrollmentIds,
                request.ExamEnrollmentIds,
                request.Language),
            cancellationToken);

        return result == null
            ? NotFound()
            : File(result.Content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.FileName);
    }

    [HttpGet("youths/{youthId:guid}")]
    public async Task<IActionResult> GetYouthReport(
        Guid youthId,
        [FromQuery] Guid? catechismYearId,
        [FromQuery] Guid? enrollmentId,
        CancellationToken cancellationToken = default)
    {
        YouthReportDto? result = await mediator.Send(
            new GetYouthReportQuery(youthId, catechismYearId, enrollmentId),
            cancellationToken);

        return result == null ? NotFound() : Ok(result);
    }
}