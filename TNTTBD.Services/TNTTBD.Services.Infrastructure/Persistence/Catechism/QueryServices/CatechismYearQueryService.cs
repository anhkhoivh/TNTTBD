using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Application.CatechismYears.DTOs;
using TNTTBD.Services.Application.CatechismYears.Interfaces;
using TNTTBD.Services.Domain.Common;
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

    #endregion
}