using MediatR;

namespace TNTTBD.Services.Application.Exams.Commands;

public record SaveExamResultsCommand(
    Guid ExamId,
    List<ExamResultItem> Results
) : IRequest;

public record ExamResultItem(
    Guid CatechismClassEucharisticYouthId,
    double? Score,
    bool IsDone,
    string? Remark
);
