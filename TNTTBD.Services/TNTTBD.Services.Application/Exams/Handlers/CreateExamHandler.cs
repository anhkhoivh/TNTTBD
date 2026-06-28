using MediatR;
using TNTTBD.Services.Application.Exams.Commands;
using TNTTBD.Services.Domain.Catechism;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.Exams.Handlers;

public class CreateExamHandler : IRequestHandler<CreateExamCommand, Guid>
{
    private readonly IExamRepository _repository;

    public CreateExamHandler(IExamRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateExamCommand command, CancellationToken cancellationToken)
    {
        Exam exam = new(Guid.NewGuid(), command.CatechismClassId, command.ExamDate, command.Title, []);
        await _repository.AddAsync(exam, cancellationToken);
        return exam.Id;
    }
}
