using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Application.CatechismClassEucharisticYouths.DTOs;
using TNTTBD.Services.Application.CatechismClassEucharisticYouths.Interfaces;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.QueryServices;

public class CatechismClassEucharisticYouthQueryService : ICatechismClassEucharisticYouthQueryService
{
    private readonly CatechismDbContext _context;

    public CatechismClassEucharisticYouthQueryService(CatechismDbContext context)
    {
        _context = context;
    }

    public async Task<CatechismClassEucharisticYouthDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.CatechismClassEucharisticYouths
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Join(_context.EucharisticYouths, e => e.EucharisticYouthId, y => y.Id, (e, y) => new { e, y })
            .Join(_context.CatechismClasses, ey => ey.e.CatechismClassId, c => c.Id, (ey, c) => new { ey.e, ey.y, c })
            .Join(_context.CatechismYears, eyc => eyc.c.CatechismYearId, yr => yr.Id, (eyc, yr) => new CatechismClassEucharisticYouthDto
            {
                Id = eyc.e.Id,
                CatechismClassId = eyc.e.CatechismClassId,
                ClassName = eyc.c.ClassName,
                CatechismYearTheme = yr.Theme ?? string.Empty,
                EucharisticYouthId = eyc.e.EucharisticYouthId,
                Code = eyc.y.Code,
                ChristName = eyc.y.FullName.ChristName,
                FamilyName = eyc.y.FullName.FamilyName,
                MiddleName = eyc.y.FullName.MiddleName,
                GivenName = eyc.y.FullName.GivenName,
                EnrolledAt = eyc.e.EnrolledAt,
                LeftAt = eyc.e.LeftAt,
                Status = eyc.e.Status,
                Note = eyc.e.Note,
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<CatechismClassEucharisticYouthDto>> GetCatechismClassEucharisticYouthsByClassAsync(Guid classId, CancellationToken cancellationToken)
    {
        return await _context.CatechismClassEucharisticYouths
            .AsNoTracking()
            .Where(e => e.CatechismClassId == classId)
            .Join(_context.EucharisticYouths, e => e.EucharisticYouthId, y => y.Id, (e, y) => new { e, y })
            .Join(_context.CatechismClasses, ey => ey.e.CatechismClassId, c => c.Id, (ey, c) => new { ey.e, ey.y, c })
            .Join(_context.CatechismYears, eyc => eyc.c.CatechismYearId, yr => yr.Id, (eyc, yr) => new CatechismClassEucharisticYouthDto
            {
                Id = eyc.e.Id,
                CatechismClassId = eyc.e.CatechismClassId,
                ClassName = eyc.c.ClassName,
                CatechismYearTheme = yr.Theme ?? string.Empty,
                EucharisticYouthId = eyc.e.EucharisticYouthId,
                Code = eyc.y.Code,
                ChristName = eyc.y.FullName.ChristName,
                FamilyName = eyc.y.FullName.FamilyName,
                MiddleName = eyc.y.FullName.MiddleName,
                GivenName = eyc.y.FullName.GivenName,
                EnrolledAt = eyc.e.EnrolledAt,
                LeftAt = eyc.e.LeftAt,
                Status = eyc.e.Status,
                Note = eyc.e.Note,
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<List<CatechismClassEucharisticYouthDto>> GetCatechismClassEucharisticYouthsByYouthAsync(Guid youthId, CancellationToken cancellationToken)
    {
        return await _context.CatechismClassEucharisticYouths
            .AsNoTracking()
            .Where(e => e.EucharisticYouthId == youthId)
            .Join(_context.EucharisticYouths, e => e.EucharisticYouthId, y => y.Id, (e, y) => new { e, y })
            .Join(_context.CatechismClasses, ey => ey.e.CatechismClassId, c => c.Id, (ey, c) => new { ey.e, ey.y, c })
            .Join(_context.CatechismYears, eyc => eyc.c.CatechismYearId, yr => yr.Id, (eyc, yr) => new CatechismClassEucharisticYouthDto
            {
                Id = eyc.e.Id,
                CatechismClassId = eyc.e.CatechismClassId,
                ClassName = eyc.c.ClassName,
                CatechismYearTheme = yr.Theme ?? string.Empty,
                EucharisticYouthId = eyc.e.EucharisticYouthId,
                Code = eyc.y.Code,
                ChristName = eyc.y.FullName.ChristName,
                FamilyName = eyc.y.FullName.FamilyName,
                MiddleName = eyc.y.FullName.MiddleName,
                GivenName = eyc.y.FullName.GivenName,
                EnrolledAt = eyc.e.EnrolledAt,
                LeftAt = eyc.e.LeftAt,
                Status = eyc.e.Status,
                Note = eyc.e.Note,
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<PaginatedResult<CatechismClassEucharisticYouthDto>> GetAllAsync(Guid? classId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var query = _context.CatechismClassEucharisticYouths.AsNoTracking();

        if (classId.HasValue)
            query = query.Where(e => e.CatechismClassId == classId.Value);

        int total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(e => e.EnrolledAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Join(_context.EucharisticYouths, e => e.EucharisticYouthId, y => y.Id, (e, y) => new { e, y })
            .Join(_context.CatechismClasses, ey => ey.e.CatechismClassId, c => c.Id, (ey, c) => new { ey.e, ey.y, c })
            .Join(_context.CatechismYears, eyc => eyc.c.CatechismYearId, yr => yr.Id, (eyc, yr) => new CatechismClassEucharisticYouthDto
            {
                Id = eyc.e.Id,
                CatechismClassId = eyc.e.CatechismClassId,
                ClassName = eyc.c.ClassName,
                CatechismYearTheme = yr.Theme ?? string.Empty,
                EucharisticYouthId = eyc.e.EucharisticYouthId,
                Code = eyc.y.Code,
                ChristName = eyc.y.FullName.ChristName,
                FamilyName = eyc.y.FullName.FamilyName,
                MiddleName = eyc.y.FullName.MiddleName,
                GivenName = eyc.y.FullName.GivenName,
                EnrolledAt = eyc.e.EnrolledAt,
                LeftAt = eyc.e.LeftAt,
                Status = eyc.e.Status,
                Note = eyc.e.Note,
            })
            .ToListAsync(cancellationToken);

        return new PaginatedResult<CatechismClassEucharisticYouthDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize
        };
    }
}