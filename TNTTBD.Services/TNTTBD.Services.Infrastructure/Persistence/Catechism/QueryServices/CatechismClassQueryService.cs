using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Application.CatechismClasses.DTOs;
using TNTTBD.Services.Application.CatechismClasses.Interfaces;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.QueryServices;

public class CatechismClassQueryService : ICatechismClassQueryService
{
    private readonly CatechismDbContext _context;

    public CatechismClassQueryService(CatechismDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<CatechismClassDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.CatechismClasses
            .Where(x => x.Id == id)
            .Select(entity => new CatechismClassDto
            {
                Id = entity.Id,
                ClassName = entity.ClassName,
                CatechismYearId = entity.CatechismYearId,
                CatechismYearTitle = $"{entity.CatechismYear.StartDate.Year}-{entity.CatechismYear.EndDate.Year}"
            })
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<CatechismClassDto>> GetClassesByYearAsync(Guid yearId, CancellationToken cancellationToken)
    {
        return await _context.CatechismClasses
            .Where(c => c.CatechismYearId == yearId)
            .Select(entity => new CatechismClassDto
            {
                Id = entity.Id,
                ClassName = entity.ClassName,
                CatechismYearId = entity.CatechismYearId,
                CatechismYearTitle = $"{entity.CatechismYear.StartDate.Year}-{entity.CatechismYear.EndDate.Year}"
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<CatechismClassDto>> GetAllAsync(Guid? yearId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var query = _context.CatechismClasses.AsNoTracking();

        if (yearId.HasValue)
            query = query.Where(c => c.CatechismYearId == yearId.Value);

        int total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(c => c.ClassName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(entity => new CatechismClassDto
            {
                Id = entity.Id,
                ClassName = entity.ClassName,
                CatechismYearId = entity.CatechismYearId,
                CatechismYearTitle = $"{entity.CatechismYear.StartDate.Year}-{entity.CatechismYear.EndDate.Year}"
            })
            .ToListAsync(cancellationToken);

        return new PaginatedResult<CatechismClassDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize
        };
    }
}