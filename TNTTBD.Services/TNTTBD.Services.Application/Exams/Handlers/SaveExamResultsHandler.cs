using MediatR;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Exams.Commands;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.Exams.Handlers;

public class SaveExamResultsHandler : IRequestHandler<SaveExamResultsCommand>
{
    private readonly IExamRepository _repository;
    private readonly IAuthorizationContext _authContext;

    public SaveExamResultsHandler(IExamRepository repository, IAuthorizationContext authContext)
    {
        _repository = repository;
        _authContext = authContext;
    }

    public async Task Handle(SaveExamResultsCommand command, CancellationToken cancellationToken)
    {
        var markedBy = await _authContext.GetCurrentCatechistIdAsync(cancellationToken)
            ?? throw new UnauthorizedAccessException("Only a catechist can save exam results.");

        var results = command.Results.Select(r => (
            r.CatechismClassEucharisticYouthId,
            r.Score,
            r.Remark,
            markedBy
        )).ToList();

        await _repository.SaveResults(command.ExamId, results, cancellationToken);
    }
}
