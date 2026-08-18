using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Domain.Catechism;
using TNTTBD.Services.Domain.Catechism.JoinModels;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.Common;
using TNTTBD.Services.Domain.Enums;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Repositories;

public class CatechismClassRepository : ICatechismClassRepository
{
    #region Fields

    private readonly CatechismDbContext _context;

    #endregion

    #region Constructors

    public CatechismClassRepository(CatechismDbContext context)
    {
        _context = context;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public async Task AddAsync(CatechismClass catechismClass, CancellationToken cancellationToken)
    {
        CatechismClassEntity entity = catechismClass.ToEntity();
        await _context.CatechismClasses.AddAsync(entity, cancellationToken);
    }

    /// <inheritdoc />
    public async Task Update(Guid id, string className, string classCode, CancellationToken cancellationToken)
    {
        CatechismClassEntity? entity = await _context.CatechismClasses.FirstOrDefaultAsync(y => y.Id == id, cancellationToken);
        if (entity == null)
        {
            throw new DomainException(DomainErrors.ValidationNotFound);
        }

        CatechismClass domain = entity.ToDomain();
        domain.UpdateInfo(className, classCode);
        domain.UpdateEntity(entity);
    }

    /// <inheritdoc />
    public async Task Remove(Guid id, CancellationToken cancellationToken)
    {
        CatechismClassEntity? entity = await _context.CatechismClasses
            .FirstOrDefaultAsync(y => y.Id == id, cancellationToken);
        if (entity == null)
        {
            throw new DomainException(DomainErrors.ValidationNotFound);
        }
        _context.CatechismClasses.Remove(entity);
    }

    /// <inheritdoc />
    public async Task MarkAttendanceList(
        Guid sessionId,
        IEnumerable<(Guid CatechismClassEucharisticYouthId, AttendanceStatus Status)> attendances,
        Guid catechistId,
        CancellationToken cancellationToken)
    {
        CatechismSessionEntity? entity = await _context.CatechismSessions.Include(s => s.EucharisticYouthAttendances).FirstOrDefaultAsync(y => y.Id == sessionId, cancellationToken);
        if (entity == null)
        {
            throw new DomainException(DomainErrors.ValidationNotFound);
        }

        CatechismSession domain = entity.ToDomain();
        domain.MarkAttendances(attendances, catechistId);
        domain.UpdateAttendances(entity);
    }

    #endregion
}