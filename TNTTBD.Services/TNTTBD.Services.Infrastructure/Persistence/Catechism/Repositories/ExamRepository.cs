using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Domain.Catechism;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.Common;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Repositories;

public class ExamRepository : IExamRepository
{
    private readonly CatechismDbContext _context;

    public ExamRepository(CatechismDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Exam exam, CancellationToken cancellationToken)
    {
        ExamEntity entity = exam.ToEntity();
        await _context.Exams.AddAsync(entity, cancellationToken);
    }

    public async Task Update(Guid id, string title, DateOnly examDate, CancellationToken cancellationToken)
    {
        ExamEntity? entity = await _context.Exams.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (entity == null)
            throw new DomainException(DomainErrors.ValidationNotFound);

        entity.Title = title;
        entity.ExamDate = examDate;
    }

    public async Task Remove(Guid id, CancellationToken cancellationToken)
    {
        ExamEntity? entity = await _context.Exams
            .Include(e => e.ExamResults)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        if (entity == null)
            throw new DomainException(DomainErrors.ValidationNotFound);

        _context.ExamResults.RemoveRange(entity.ExamResults);
        _context.Exams.Remove(entity);
    }

    public async Task SaveResults(Guid examId, List<(Guid youthId, double score, bool isDone, string? remark, Guid markedBy)> results, CancellationToken cancellationToken)
    {
        ExamEntity? exam = await _context.Exams
            .Include(e => e.ExamResults)
            .FirstOrDefaultAsync(e => e.Id == examId, cancellationToken);
        if (exam == null)
            throw new DomainException(DomainErrors.ValidationNotFound);

        foreach (var (youthId, score, isDone, remark, markedBy) in results)
        {
            var existing = exam.ExamResults.FirstOrDefault(r => r.CatechismClassEucharisticYouthId == youthId);
            if (existing != null)
            {
                existing.Score = score;
                existing.IsDone = isDone;
                existing.Remark = remark;
                existing.MarkedByCatechistId = markedBy;
                existing.MarkedTime = DateTimeOffset.UtcNow;
            }
            else
            {
                exam.ExamResults.Add(new ExamResultEntity
                {
                    Id = Guid.NewGuid(),
                    ExamId = examId,
                    CatechismClassEucharisticYouthId = youthId,
                    Score = score,
                    IsDone = isDone,
                    Remark = remark,
                    MarkedByCatechistId = markedBy,
                    MarkedTime = DateTimeOffset.UtcNow
                });
            }
        }
    }
}
