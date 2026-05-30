using MediatR;
using TNTTBD.Services.Application.Exams.Commands;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.Exams.Handlers;

public class SaveExamResultsHandler : IRequestHandler<SaveExamResultsCommand>
{
    private readonly IExamRepository _repository;

    public SaveExamResultsHandler(IExamRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(SaveExamResultsCommand command, CancellationToken cancellationToken)
    {
        var results = command.Results.Select(r => (
            r.CatechismClassEucharisticYouthId,
            r.Score ?? 0,
            r.IsDone,
            r.Remark,
            Guid.Empty // TODO: inject current catechist id from auth context
        )).ToList();

        await _repository.SaveResults(command.ExamId, results, cancellationToken);
    }
}
