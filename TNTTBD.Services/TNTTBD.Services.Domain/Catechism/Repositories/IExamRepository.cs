using TNTTBD.Services.Domain.Catechism;

namespace TNTTBD.Services.Domain.Catechism.Repositories;

public interface IExamRepository
{
    Task AddAsync(Exam exam, CancellationToken cancellationToken);
    Task<Guid?> GetClassIdAsync(Guid examId, CancellationToken cancellationToken);
    Task Update(Guid id, string title, DateOnly examDate, CancellationToken cancellationToken);
    Task Remove(Guid id, CancellationToken cancellationToken);
    Task SaveResults(Guid examId, List<(Guid youthId, double? score, string? remark, Guid markedBy)> results, CancellationToken cancellationToken);
}
