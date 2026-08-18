using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.Exams.DTOs;
using TNTTBD.Services.Application.Exams.Interfaces;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.QueryServices;

public class ExamQueryService : IExamQueryService
{
    private readonly CatechismDbContext _context;

    public ExamQueryService(CatechismDbContext context)
    {
        _context = context;
    }

    public async Task<ExamDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var examHeader = await _context.Exams
            .Where(e => e.Id == id)
            .AsNoTracking()
            .Select(e => new { e.Id, e.Title, e.CatechismClassId, e.ExamDate })
            .FirstOrDefaultAsync(cancellationToken);

        if (examHeader == null) return null;

        var existingResults = await _context.ExamResults
            .Where(r => r.ExamId == id)
            .AsNoTracking()
            .Select(r => new { r.Id, r.CatechismClassEucharisticYouthId, r.Score, r.IsDone, r.Remark })
            .ToListAsync(cancellationToken);

        var resultLookup = existingResults.ToDictionary(r => r.CatechismClassEucharisticYouthId);

        var enrollments = await _context.CatechismClassEucharisticYouths
            .Where(cy => cy.CatechismClassId == examHeader.CatechismClassId && cy.Status == EnrollmentStatus.Active)
            .AsNoTracking()
            .Select(cy => new { cy.Id, cy.EucharisticYouth.Code, cy.EucharisticYouth.FullName })
            .ToListAsync(cancellationToken);

        var flatResults = enrollments.Select(e =>
        {
            resultLookup.TryGetValue(e.Id, out var r);
            return new ExamResultDto
            {
                Id = r?.Id,
                CatechismClassEucharisticYouthId = e.Id,
                YouthCode = e.Code,
                YouthFullName = e.FullName,
                Score = r?.Score,
                IsDone = r?.IsDone ?? false,
                Remark = r?.Remark,
            };
        }).ToList();

        return new ExamDto
        {
            Id = examHeader.Id,
            Title = examHeader.Title,
            CatechismClassId = examHeader.CatechismClassId,
            ExamDate = examHeader.ExamDate,
            ExamResults = flatResults
        };
    }

    public async Task<PaginatedResult<ExamDto>> GetAllAsync(Guid? classId, int page, int? pageSize, CancellationToken cancellationToken)
    {
        var query = _context.Exams.AsNoTracking();

        if (classId.HasValue)
        {
            query = query.Where(e => e.CatechismClassId == classId.Value);
        }

        int total = await query.CountAsync(cancellationToken);

        if (pageSize.HasValue)
        {
            query = query.Skip((page - 1) * pageSize.Value).Take(pageSize.Value);
        }

        var items = await query
            .OrderByDescending(e => e.ExamDate)
            .Select(e => new ExamDto
            {
                Id = e.Id,
                Title = e.Title,
                CatechismClassId = e.CatechismClassId,
                ExamDate = e.ExamDate,
                ExamResults = e.ExamResults.Select(r => new ExamResultDto
                {
                    Id = r.Id,
                    CatechismClassEucharisticYouthId = r.CatechismClassEucharisticYouthId,
                    Score = r.Score,
                    IsDone = r.IsDone,
                    Remark = r.Remark,
                }).ToList()
            })
            .ToListAsync(cancellationToken);

        return new PaginatedResult<ExamDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize ?? total
        };
    }

    public async Task<PaginatedResult<MyExamResultDto>> GetMyResultsAsync(Guid youthId, int page, int? pageSize, CancellationToken cancellationToken)
    {
        // Find all enrollments for this youth
        var enrollmentIds = await _context.CatechismClassEucharisticYouths
            .AsNoTracking()
            .Where(e => e.EucharisticYouthId == youthId)
            .Select(e => e.Id)
            .ToListAsync(cancellationToken);

        var query = _context.ExamResults
            .AsNoTracking()
            .Where(r => enrollmentIds.Contains(r.CatechismClassEucharisticYouthId));

        int total = await query.CountAsync(cancellationToken);

        if (pageSize.HasValue)
        {
            query = query.Skip((page - 1) * pageSize.Value).Take(pageSize.Value);
        }

        var items = await query
            .Join(_context.Exams, r => r.ExamId, e => e.Id, (r, e) => new MyExamResultDto
            {
                ExamId = e.Id,
                ExamTitle = e.Title,
                ExamDate = e.ExamDate,
                Score = r.Score,
                IsDone = r.IsDone,
                Remark = r.Remark
            })
            .OrderByDescending(x => x.ExamDate)
            .ToListAsync(cancellationToken);

        return new PaginatedResult<MyExamResultDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize ?? total
        };
    }
}
