using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Exams.Commands;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.Exams.Handlers;

public class RemoveExamHandler : IRequestHandler<RemoveExamCommand>
{
    private readonly IExamRepository _repository;

    public RemoveExamHandler(IExamRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(RemoveExamCommand command, CancellationToken cancellationToken)
    {
        await _repository.Remove(command.Id, cancellationToken);
    }
}
