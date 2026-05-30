using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Application.CatechismYears.DTOs;
using TNTTBD.Services.Application.CatechismYears.Interfaces;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

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
            .Select(y => y.ToDTO())
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<CatechismYearDto>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        var query = _context.CatechismYears.AsNoTracking();

        int total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(y => y.StartDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(y => y.CatechismClasses)
            .Select(y => y.ToDTO())
            .ToListAsync(cancellationToken);

        return new PaginatedResult<CatechismYearDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize
        };
    }

    #endregion
}