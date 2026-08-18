using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Application.CatechismClasses.DTOs;
using TNTTBD.Services.Application.CatechismYears.DTOs;
using TNTTBD.Services.Application.CatechismYears.Interfaces;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.QueryServices;

public class CatechismYearQueryService : ICatechismYearQueryService
{
    #region Fields

    private readonly CatechismDbContext _context;

    #endregion

    #region Constructors

    public CatechismYearQueryService(CatechismDbContext context)
    {
        _context = context;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public async Task<CatechismYearDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.CatechismYears
            .Where(y => y.Id == id)
            .Select(entity => new CatechismYearDto
            {
                Id = entity.Id,
                Title = $"{entity.StartDate.Year}-{entity.EndDate.Year}",
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Theme = entity.Theme,
                CatechismClasses = entity.CatechismClasses.Select(e => new CatechismClassDto
                {
                    Id = e.Id,
                    ClassName = e.ClassName,
                    ClassCode = e.ClassCode,
                    CatechismYearId = e.CatechismYearId,
                    CatechismYearTitle = $"{e.CatechismYear.StartDate.Year}-{e.CatechismYear.EndDate.Year}"
                }).ToList()
            })
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<CatechismYearDto>> GetAllAsync(int page, int? pageSize, CancellationToken cancellationToken)
    {
        var query = _context.CatechismYears.AsNoTracking();

        int total = await query.CountAsync(cancellationToken);

        if (pageSize.HasValue)
        {
            query = query.Skip((page - 1) * pageSize.Value).Take(pageSize.Value);
        }

        var items = await query
            .OrderByDescending(y => y.StartDate)
            .Include(y => y.CatechismClasses)
            .Select(entity => new CatechismYearDto
            {
                Id = entity.Id,
                Title = $"{entity.StartDate.Year}-{entity.EndDate.Year}",
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Theme = entity.Theme,
                CatechismClasses = entity.CatechismClasses.Select(e => new CatechismClassDto
                {
                    Id = e.Id,
                    ClassName = e.ClassName,
                    ClassCode = e.ClassCode,
                    CatechismYearId = e.CatechismYearId,
                    CatechismYearTitle = $"{e.CatechismYear.StartDate.Year}-{e.CatechismYear.EndDate.Year}"
                }).ToList()
            })
            .ToListAsync(cancellationToken);

        return new PaginatedResult<CatechismYearDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize ?? total
        };
    }

    #endregion
}