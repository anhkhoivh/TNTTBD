using MediatR;

namespace TNTTBD.Services.Application.Exams.Commands;

public record SaveExamResultsCommand(
    Guid ExamId,
    List<ExamResultItem> Results
) : IRequest<bool>;

public record ExamResultItem(
    Guid CatechismClassEucharisticYouthId,
    double? Score,
    string? Remark
);
