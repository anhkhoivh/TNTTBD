using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Application.CatechismClasses.DTOs;
using TNTTBD.Services.Application.CatechismClasses.Interfaces;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Domain.Enums;

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
                ClassCode = entity.ClassCode,
                CatechismYearId = entity.CatechismYearId,
                CatechismYearTitle = $"{entity.CatechismYear.StartDate.Year}-{entity.CatechismYear.EndDate.Year}",
                ActiveTotal = entity.CatechismClassEucharisticYouths.Count(cy => cy.Status == EnrollmentStatus.Active)
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
                ClassCode = entity.ClassCode,
                CatechismYearId = entity.CatechismYearId,
                CatechismYearTitle = $"{entity.CatechismYear.StartDate.Year}-{entity.CatechismYear.EndDate.Year}",
                ActiveTotal = entity.CatechismClassEucharisticYouths.Count(cy => cy.Status == EnrollmentStatus.Active)
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<CatechismClassDto>> GetAllAsync(Guid? yearId, int page, int? pageSize, CancellationToken cancellationToken)
    {
        var query = _context.CatechismClasses.AsNoTracking();

        if (yearId.HasValue)
            query = query.Where(c => c.CatechismYearId == yearId.Value);

        int total = await query.CountAsync(cancellationToken);

        if (pageSize.HasValue)
        {
            query = query.Skip((page - 1) * pageSize.Value).Take(pageSize.Value);
        }

        var items = await query
            .OrderBy(c => c.ClassName)
            .Select(entity => new CatechismClassDto
            {
                Id = entity.Id,
                ClassName = entity.ClassName,
                ClassCode = entity.ClassCode,
                CatechismYearId = entity.CatechismYearId,
                CatechismYearTitle = $"{entity.CatechismYear.StartDate.Year}-{entity.CatechismYear.EndDate.Year}",
                ActiveTotal = entity.CatechismClassEucharisticYouths.Count(cy => cy.Status == EnrollmentStatus.Active)
            })
            .ToListAsync(cancellationToken);

        return new PaginatedResult<CatechismClassDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize ?? total
        };
    }
}