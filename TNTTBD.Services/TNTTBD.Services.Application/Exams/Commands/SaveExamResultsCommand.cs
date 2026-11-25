namespace TNTTBD.Services.Application.Exams.Commands;

public record SaveExamResultsCommand(
    Guid ExamId,
    List<ExamResultItem> Results
);

public record ExamResultItem(
    Guid CatechismClassEucharisticYouthId,
    double? Score,
    string? Remark
);
