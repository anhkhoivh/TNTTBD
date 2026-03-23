using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Domain.Catechism;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.Common;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Repositories;

public class CatechismYearRepository : ICatechismYearRepository
{
    #region Fields

    private readonly CatechismDbContext _context;

    #endregion

    #region Constructors

    public CatechismYearRepository(CatechismDbContext context)
    {
        _context = context;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public async Task AddAsync(CatechismYear catechismYear, CancellationToken cancellationToken)
    {
        CatechismYearEntity entity = catechismYear.ToEntity();
        await _context.CatechismYears.AddAsync(entity, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<CatechismYear?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        CatechismYearEntity? entity = await _context.CatechismYears
            .Include(y => y.CatechismClasses)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity?.ToDomain();
    }

    /// <inheritdoc />
    public async Task Update(Guid id, DateOnly startDate, DateOnly endDate, string? theme, CancellationToken cancellationToken)
    {
        CatechismYearEntity? yearEntity = await _context.CatechismYears.FirstOrDefaultAsync(y => y.Id == id, cancellationToken);
        if (yearEntity == null)
        {
            throw new Exception(DomainErrors.ValidationNotFound);
        }

        CatechismYear domain = yearEntity.ToDomain();
        domain.UpdateInfo(startDate, endDate, theme);
        domain.UpdateEntity(yearEntity);
    }

    /// <inheritdoc />
    public async Task Remove(Guid id, CancellationToken cancellationToken)
    {
        CatechismYearEntity? yearEntity = await _context.CatechismYears
            .FirstOrDefaultAsync(y => y.Id == id, cancellationToken);
        if (yearEntity == null)
        {
            throw new Exception(DomainErrors.ValidationNotFound);
        }
        _context.Remove(yearEntity);
    }

    #endregion
}