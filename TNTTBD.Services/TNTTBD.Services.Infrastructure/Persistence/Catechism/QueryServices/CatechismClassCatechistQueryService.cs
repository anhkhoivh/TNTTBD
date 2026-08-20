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
            .Select(entity => new CatechismClassCatechistDto
            {
                Id = entity.Id,
                CatechismClassId = entity.CatechismClassId,
                ClassName = entity.CatechismClass.ClassName,
                CatechismYearTheme = entity.CatechismClass.CatechismYear.Theme ?? string.Empty,
                CatechistId = entity.CatechistId,
                FullName = entity.Catechist.FullName,
                Role = entity.Role,
                EnrolledAt = entity.EnrolledAt,
                LeftAt = entity.LeftAt,
                Status = entity.Status,
                Note = entity.Note,
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<CatechismClassCatechistDto>> GetByClassAsync(Guid classId, CancellationToken cancellationToken)
    {
        return await _context.CatechismClassCatechists
            .AsNoTracking()
            .Where(e => e.CatechismClassId == classId)
            .Select(entity => new CatechismClassCatechistDto
            {
                Id = entity.Id,
                CatechismClassId = entity.CatechismClassId,
                ClassName = entity.CatechismClass.ClassName,
                CatechismYearTheme = entity.CatechismClass.CatechismYear.Theme ?? string.Empty,
                CatechistId = entity.CatechistId,
                FullName = entity.Catechist.FullName,
                Role = entity.Role,
                EnrolledAt = entity.EnrolledAt,
                LeftAt = entity.LeftAt,
                Status = entity.Status,
                Note = entity.Note,
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<List<CatechismClassCatechistDto>> GetByCatechistAsync(Guid catechistId, CancellationToken cancellationToken)
    {
        return await _context.CatechismClassCatechists
            .AsNoTracking()
            .Where(e => e.CatechistId == catechistId)
            .Select(entity => new CatechismClassCatechistDto
            {
                Id = entity.Id,
                CatechismClassId = entity.CatechismClassId,
                ClassName = entity.CatechismClass.ClassName,
                CatechismYearTheme = entity.CatechismClass.CatechismYear.Theme ?? string.Empty,
                CatechistId = entity.CatechistId,
                FullName = entity.Catechist.FullName,
                Role = entity.Role,
                EnrolledAt = entity.EnrolledAt,
                LeftAt = entity.LeftAt,
                Status = entity.Status,
                Note = entity.Note,
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<PaginatedResult<CatechismClassCatechistDto>> GetAllAsync(Guid? classId, Guid? catechistId, int page, int? pageSize, CancellationToken cancellationToken)
    {
        var query = _context.CatechismClassCatechists.AsNoTracking();

        if (classId.HasValue)
            query = query.Where(e => e.CatechismClassId == classId.Value);

        if (catechistId.HasValue)
            query = query.Where(e => e.CatechistId == catechistId.Value);

        int total = await query.CountAsync(cancellationToken);

        if (pageSize.HasValue)
        {
            query = query.Skip((page - 1) * pageSize.Value).Take(pageSize.Value);
        }

        var items = await query
            .OrderBy(e => e.EnrolledAt)
            .Select(entity => new CatechismClassCatechistDto
            {
                Id = entity.Id,
                CatechismClassId = entity.CatechismClassId,
                ClassName = entity.CatechismClass.ClassName,
                CatechismYearTheme = entity.CatechismClass.CatechismYear.Theme ?? string.Empty,
                CatechistId = entity.CatechistId,
                FullName = entity.Catechist.FullName,
                Role = entity.Role,
                EnrolledAt = entity.EnrolledAt,
                LeftAt = entity.LeftAt,
                Status = entity.Status,
                Note = entity.Note,
            })
            .ToListAsync(cancellationToken);

        return new PaginatedResult<CatechismClassCatechistDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize ?? total
        };
    }
}
