using MediatR;
using TNTTBD.Services.Application.Reports.DTOs;

namespace TNTTBD.Services.Application.Reports.Queries;

public record GetYouthReportQuery(
    Guid YouthId,
    Guid? CatechismYearId = null,
    Guid? EnrollmentId = null
) : IRequest<YouthReportDto?>;