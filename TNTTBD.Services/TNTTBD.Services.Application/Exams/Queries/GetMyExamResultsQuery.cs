namespace TNTTBD.Services.Application.Exams.Queries;

public record GetMyExamResultsQuery(
    Guid YouthId,
    int Page = 1,
    int? PageSize = null
);
