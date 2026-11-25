namespace TNTTBD.Services.Application.Exams.Commands;

public record CreateExamCommand(
    string Title,
    Guid CatechismClassId,
    DateOnly ExamDate
);
