using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Exams.Commands;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.Exams.Handlers;

public class UpdateExamHandler : IRequestHandler<UpdateExamCommand>
{
    private readonly IExamRepository _repository;

    public UpdateExamHandler(IExamRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateExamCommand command, CancellationToken cancellationToken)
    {
        await _repository.Update(command.Id, command.Title, command.ExamDate, cancellationToken);
    }
}
