using MediatR;

namespace TNTTBD.Services.Application.Exams.Commands;

public record UpdateExamCommand(
    Guid Id,
    string Title,
    DateOnly ExamDate
) : IRequest;
