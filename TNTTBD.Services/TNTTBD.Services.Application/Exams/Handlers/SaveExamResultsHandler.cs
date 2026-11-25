using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Exams.Commands;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.Common;

namespace TNTTBD.Services.Application.Exams.Handlers;

public class SaveExamResultsHandler : IRequestHandler<SaveExamResultsCommand, bool>
{
    private const double MinScore = 0;
    private const double MaxScore = 10;
    private const int MaxRemarkLength = 500;

    private readonly IExamRepository _repository;
    private readonly IAuthorizationContext _authContext;

    public SaveExamResultsHandler(IExamRepository repository, IAuthorizationContext authContext)
    {
        _repository = repository;
        _authContext = authContext;
    }

    public async Task<bool> Handle(SaveExamResultsCommand command, CancellationToken cancellationToken)
    {
        if (command.Results is null || command.Results.Count == 0)
        {
            throw new DomainException(DomainErrors.ValidationRequired);
        }

        foreach (var r in command.Results)
        {
            if (r.Score is { } s && (!double.IsFinite(s) || s < MinScore || s > MaxScore))
            {
                throw new DomainException(DomainErrors.ExamScoreOutOfRange);
            }
            if (r.Remark is { Length: > MaxRemarkLength })
            {
                throw new DomainException(DomainErrors.ExamRemarkTooLong);
            }
        }

        Guid classId = await _repository.GetClassIdAsync(command.ExamId, cancellationToken)
            ?? throw new DomainException(DomainErrors.ValidationNotFound);

        if (!await _authContext.CanAccessClassAsync(classId, cancellationToken))
        {
            return false;
        }

        var markedBy = await _authContext.GetCurrentCatechistIdAsync(cancellationToken)
            ?? throw new UnauthorizedAccessException("Only a catechist can save exam results.");

        var results = command.Results.Select(r => (
            r.CatechismClassEucharisticYouthId,
            r.Score,
            r.Remark,
            markedBy
        )).ToList();

        await _repository.SaveResults(command.ExamId, results, cancellationToken);
        return true;
    }
}
