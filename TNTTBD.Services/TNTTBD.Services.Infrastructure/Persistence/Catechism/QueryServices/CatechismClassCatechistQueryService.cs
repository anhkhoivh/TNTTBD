using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Application.CatechismClassCatechists.DTOs;
using TNTTBD.Services.Application.CatechismClassCatechists.Interfaces;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.QueryServices;

public class CatechismClassCatechistQueryService : ICatechismClassCatechistQueryService
{
    private readonly CatechismDbContext _context;

    public CatechismClassCatechistQueryService(CatechismDbContext context)
    {
        _context = context;
    }

    public async Task<CatechismClassCatechistDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.CatechismClassCatechists
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Join(_context.Catechists, e => e.CatechistId, c => c.Id, (e, c) => new { e, c })
            .Join(_context.CatechismClasses, ec => ec.e.CatechismClassId, cls => cls.Id, (ec, cls) => new { ec.e, ec.c, cls })
            .Join(_context.CatechismYears, ecl => ecl.cls.CatechismYearId, yr => yr.Id, (ecl, yr) => new CatechismClassCatechistDto
            {
                Id = ecl.e.Id,
                CatechismClassId = ecl.e.CatechismClassId,
                ClassName = ecl.cls.ClassName,
                CatechismYearTheme = yr.Theme ?? string.Empty,
                CatechistId = ecl.e.CatechistId,
                ChristName = ecl.c.FullName.ChristName,
                FamilyName = ecl.c.FullName.FamilyName,
                MiddleName = ecl.c.FullName.MiddleName,
                GivenName = ecl.c.FullName.GivenName,
                Role = ecl.e.Role,
                EnrolledAt = ecl.e.EnrolledAt,
                LeftAt = ecl.e.LeftAt,
                Status = ecl.e.Status,
                Note = ecl.e.Note,
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<CatechismClassCatechistDto>> GetByClassAsync(Guid classId, CancellationToken cancellationToken)
    {
        return await _context.CatechismClassCatechists
            .AsNoTracking()
            .Where(e => e.CatechismClassId == classId)
            .Join(_context.Catechists, e => e.CatechistId, c => c.Id, (e, c) => new { e, c })
            .Join(_context.CatechismClasses, ec => ec.e.CatechismClassId, cls => cls.Id, (ec, cls) => new { ec.e, ec.c, cls })
            .Join(_context.CatechismYears, ecl => ecl.cls.CatechismYearId, yr => yr.Id, (ecl, yr) => new CatechismClassCatechistDto
            {
                Id = ecl.e.Id,
                CatechismClassId = ecl.e.CatechismClassId,
                ClassName = ecl.cls.ClassName,
                CatechismYearTheme = yr.Theme ?? string.Empty,
                CatechistId = ecl.e.CatechistId,
                ChristName = ecl.c.FullName.ChristName,
                FamilyName = ecl.c.FullName.FamilyName,
                MiddleName = ecl.c.FullName.MiddleName,
                GivenName = ecl.c.FullName.GivenName,
                Role = ecl.e.Role,
                EnrolledAt = ecl.e.EnrolledAt,
                LeftAt = ecl.e.LeftAt,
                Status = ecl.e.Status,
                Note = ecl.e.Note,
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<List<CatechismClassCatechistDto>> GetByCatechistAsync(Guid catechistId, CancellationToken cancellationToken)
    {
        return await _context.CatechismClassCatechists
            .AsNoTracking()
            .Where(e => e.CatechistId == catechistId)
            .Join(_context.Catechists, e => e.CatechistId, c => c.Id, (e, c) => new { e, c })
            .Join(_context.CatechismClasses, ec => ec.e.CatechismClassId, cls => cls.Id, (ec, cls) => new { ec.e, ec.c, cls })
            .Join(_context.CatechismYears, ecl => ecl.cls.CatechismYearId, yr => yr.Id, (ecl, yr) => new CatechismClassCatechistDto
            {
                Id = ecl.e.Id,
                CatechismClassId = ecl.e.CatechismClassId,
                ClassName = ecl.cls.ClassName,
                CatechismYearTheme = yr.Theme ?? string.Empty,
                CatechistId = ecl.e.CatechistId,
                ChristName = ecl.c.FullName.ChristName,
                FamilyName = ecl.c.FullName.FamilyName,
                MiddleName = ecl.c.FullName.MiddleName,
                GivenName = ecl.c.FullName.GivenName,
                Role = ecl.e.Role,
                EnrolledAt = ecl.e.EnrolledAt,
                LeftAt = ecl.e.LeftAt,
                Status = ecl.e.Status,
                Note = ecl.e.Note,
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<PaginatedResult<CatechismClassCatechistDto>> GetAllAsync(Guid? classId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var query = _context.CatechismClassCatechists.AsNoTracking();

        if (classId.HasValue)
            query = query.Where(e => e.CatechismClassId == classId.Value);

        int total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(e => e.EnrolledAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Join(_context.Catechists, e => e.CatechistId, c => c.Id, (e, c) => new { e, c })
            .Join(_context.CatechismClasses, ec => ec.e.CatechismClassId, cls => cls.Id, (ec, cls) => new { ec.e, ec.c, cls })
            .Join(_context.CatechismYears, ecl => ecl.cls.CatechismYearId, yr => yr.Id, (ecl, yr) => new CatechismClassCatechistDto
            {
                Id = ecl.e.Id,
                CatechismClassId = ecl.e.CatechismClassId,
                ClassName = ecl.cls.ClassName,
                CatechismYearTheme = yr.Theme ?? string.Empty,
                CatechistId = ecl.e.CatechistId,
                ChristName = ecl.c.FullName.ChristName,
                FamilyName = ecl.c.FullName.FamilyName,
                MiddleName = ecl.c.FullName.MiddleName,
                GivenName = ecl.c.FullName.GivenName,
                Role = ecl.e.Role,
                EnrolledAt = ecl.e.EnrolledAt,
                LeftAt = ecl.e.LeftAt,
                Status = ecl.e.Status,
                Note = ecl.e.Note,
            })
            .ToListAsync(cancellationToken);

        return new PaginatedResult<CatechismClassCatechistDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize
        };
    }
}
