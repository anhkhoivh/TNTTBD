namespace TNTTBD.Services.Application.Exams.Queries;

public record GetExamsQuery(
    Guid? ClassId = null,
    int Page = 1,
    int? PageSize = null
);
