using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Application.CatechismClasses.DTOs;
using TNTTBD.Services.Application.CatechismClasses.Interfaces;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.QueryServices;

public class CatechismClassQueryService : ICatechismClassQueryService
{
    #region Fields

    private readonly CatechismDbContext _context;

    #endregion

    #region Constructors

    public CatechismClassQueryService(CatechismDbContext context)
    {
        _context = context;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public async Task<CatechismClassDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.CatechismClasses
            .Where(x => x.Id == id)
            .Select(x => x.ToDTO())
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<CatechismClassDto>> GetClassesByYearAsync(Guid yearId, CancellationToken cancellationToken)
    {
        return await _context.CatechismClasses
            .Where(c => c.CatechismYearId == yearId)
            .Select(c => c.ToDTO())
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    #endregion
}