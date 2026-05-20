using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Domain.Catechism.JoinModels;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.Common;
using TNTTBD.Services.Domain.Enums;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Repositories;

public class CatechismClassEucharisticYouthRepository : ICatechismClassEucharisticYouthRepository
{
    #region Fields

    private readonly CatechismDbContext _context;

    #endregion

    #region Constructors

    public CatechismClassEucharisticYouthRepository(CatechismDbContext context)
    {
        _context = context;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public async Task AddAsync(CatechismClassEucharisticYouth domain, CancellationToken cancellationToken)
    {
        CatechismClassEucharisticYouthEntity entity = domain.ToEntity();
        await _context.CatechismClassEucharisticYouths.AddAsync(entity, cancellationToken);
    }

    /// <inheritdoc />
    public async Task Update(Guid id, EnrollmentStatus status, string note, CancellationToken cancellationToken)
    {
        var query = _context.CatechismClassEucharisticYouths.Where(y => y.Id == id);

        await query.ExecuteUpdateAsync(e =>
        {
            e.SetProperty(y => y.Status, status)
                .SetProperty(y => y.Note, note);

            if (status is EnrollmentStatus.Transferred or EnrollmentStatus.Suspended or EnrollmentStatus.Completed)
            {
                e.SetProperty(y => y.LeftAt, DateTimeOffset.UtcNow);
            }
        }, cancellationToken);
    }

    /// <inheritdoc />
    public async Task MarkCompleted(IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        await _context.CatechismClassEucharisticYouths
            .Where(y => ids.Contains(y.Id))
            .ExecuteUpdateAsync(e => e
                .SetProperty(y => y.Status, EnrollmentStatus.Completed)
                .SetProperty(y => y.LeftAt, DateTimeOffset.Now), cancellationToken);
    }

    /// <inheritdoc />
    public async Task Remove(Guid id, CancellationToken cancellationToken)
    {
        CatechismClassEucharisticYouthEntity? entity = await _context.CatechismClassEucharisticYouths.FirstOrDefaultAsync(y => y.Id == id, cancellationToken) ?? throw new Exception(DomainErrors.ValidationNotFound);
        _context.CatechismClassEucharisticYouths.Remove(entity);
    }

    #endregion
}