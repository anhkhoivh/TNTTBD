using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Application.CatechismClassEucharisticYouths.DTOs;
using TNTTBD.Services.Application.CatechismClassEucharisticYouths.Interfaces;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Domain.Enums;

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
            .OrderBy(e => e.EucharisticYouth.FullName.GivenName)
            .ThenBy(e => e.EucharisticYouth.FullName.FamilyName)
            .Select(entity => new CatechismClassEucharisticYouthDto
            {
                Id = entity.Id,
                CatechismClassId = entity.CatechismClassId,
                ClassName = entity.CatechismClass.ClassName,
                CatechismYearTheme = entity.CatechismClass.CatechismYear.Theme ?? string.Empty,
                EucharisticYouthId = entity.EucharisticYouthId,
                Code = entity.EucharisticYouth.Code,
                FullName = entity.EucharisticYouth.FullName,
                EnrolledAt = entity.EnrolledAt,
                LeftAt = entity.LeftAt,
                Status = entity.Status,
                Note = entity.Note,
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<CatechismClassEucharisticYouthDto>> GetCatechismClassEucharisticYouthsByClassAsync(Guid classId, CancellationToken cancellationToken)
    {
        return await _context.CatechismClassEucharisticYouths
            .AsNoTracking()
            .Where(e => e.CatechismClassId == classId)
            .OrderBy(e => e.EucharisticYouth.FullName.GivenName)
            .ThenBy(e => e.EucharisticYouth.FullName.FamilyName)
            .Select(entity => new CatechismClassEucharisticYouthDto
            {
                Id = entity.Id,
                CatechismClassId = entity.CatechismClassId,
                ClassName = entity.CatechismClass.ClassName,
                CatechismYearTheme = entity.CatechismClass.CatechismYear.Theme ?? string.Empty,
                EucharisticYouthId = entity.EucharisticYouthId,
                Code = entity.EucharisticYouth.Code,
                FullName = entity.EucharisticYouth.FullName,
                EnrolledAt = entity.EnrolledAt,
                LeftAt = entity.LeftAt,
                Status = entity.Status,
                Note = entity.Note,
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<List<CatechismClassEucharisticYouthDto>> GetCatechismClassEucharisticYouthsByYouthAsync(Guid youthId, CancellationToken cancellationToken)
    {
        return await _context.CatechismClassEucharisticYouths
            .AsNoTracking()
            .Where(e => e.EucharisticYouthId == youthId)
            .OrderBy(e => e.EucharisticYouth.FullName.GivenName)
            .ThenBy(e => e.EucharisticYouth.FullName.FamilyName)
            .Select(entity => new CatechismClassEucharisticYouthDto
            {
                Id = entity.Id,
                CatechismClassId = entity.CatechismClassId,
                ClassName = entity.CatechismClass.ClassName,
                CatechismYearTheme = entity.CatechismClass.CatechismYear.Theme ?? string.Empty,
                EucharisticYouthId = entity.EucharisticYouthId,
                Code = entity.EucharisticYouth.Code,
                FullName = entity.EucharisticYouth.FullName,
                EnrolledAt = entity.EnrolledAt,
                LeftAt = entity.LeftAt,
                Status = entity.Status,
                Note = entity.Note,
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<PaginatedResult<CatechismClassEucharisticYouthDto>> GetAllAsync(Guid? classId, int page, int? pageSize, CancellationToken cancellationToken)
    {
        var query = _context.CatechismClassEucharisticYouths.AsNoTracking();

        if (classId.HasValue)
            query = query.Where(e => e.CatechismClassId == classId.Value);

        int total = await query.CountAsync(cancellationToken);

        if (pageSize.HasValue)
        {
            query = query.Skip((page - 1) * pageSize.Value).Take(pageSize.Value);
        }

        var items = await query
            .OrderByDescending(e => e.Status == EnrollmentStatus.Active)
            .ThenBy(e => e.EucharisticYouth.FullName.GivenName)
            .ThenBy(e => e.EucharisticYouth.FullName.FamilyName)
            .Select(entity => new CatechismClassEucharisticYouthDto
            {
                Id = entity.Id,
                CatechismClassId = entity.CatechismClassId,
                ClassName = entity.CatechismClass.ClassName,
                CatechismYearTheme = entity.CatechismClass.CatechismYear.Theme ?? string.Empty,
                EucharisticYouthId = entity.EucharisticYouthId,
                Code = entity.EucharisticYouth.Code,
                FullName = entity.EucharisticYouth.FullName,
                EnrolledAt = entity.EnrolledAt,
                LeftAt = entity.LeftAt,
                Status = entity.Status,
                Note = entity.Note,
            })
            .ToListAsync(cancellationToken);

        return new PaginatedResult<CatechismClassEucharisticYouthDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize ?? total
        };
    }
}