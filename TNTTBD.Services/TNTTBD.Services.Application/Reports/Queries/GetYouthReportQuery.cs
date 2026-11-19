namespace TNTTBD.Services.Application.Reports.Queries;

public record GetYouthReportQuery(
    Guid YouthId,
    Guid? CatechismYearId = null,
    Guid? EnrollmentId = null
);