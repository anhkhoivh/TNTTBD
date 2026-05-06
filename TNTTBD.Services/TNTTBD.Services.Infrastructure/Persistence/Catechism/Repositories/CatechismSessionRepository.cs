using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Domain.Catechism;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.Common;
using TNTTBD.Services.Domain.Enums;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Repositories;

public class CatechismSessionRepository : ICatechismSessionRepository
{
    #region Fields

    private readonly CatechismDbContext _context;

    #endregion

    #region Constructors

    public CatechismSessionRepository(CatechismDbContext context)
    {
        _context = context;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public async Task AddAsync(CatechismSession catechismSession, CancellationToken cancellationToken)
    {
        CatechismSessionEntity entity = catechismSession.ToEntity();
        await _context.CatechismSessions.AddAsync(entity, cancellationToken);
    }

    /// <inheritdoc />
    public async Task Update(Guid id, DateTimeOffset dateTime, string title, CancellationToken cancellationToken)
    {
        CatechismSessionEntity? entity = await _context.CatechismSessions.FirstOrDefaultAsync(y => y.Id == id, cancellationToken);
        if (entity == null)
        {
            throw new Exception(DomainErrors.ValidationNotFound);
        }

        CatechismSession domain = entity.ToDomain();
        domain.UpdateInfo(dateTime, title);
        domain.UpdateEntity(entity);
    }

    /// <inheritdoc />
    public async Task Remove(Guid id, CancellationToken cancellationToken)
    {
        CatechismSessionEntity? entity = await _context.CatechismSessions
            .FirstOrDefaultAsync(y => y.Id == id, cancellationToken);
        if (entity == null)
        {
            throw new Exception(DomainErrors.ValidationNotFound);
        }
        _context.CatechismSessions.Remove(entity);
    }

    public async Task MarkAttendanceList(
        Guid sessionId,
        IEnumerable<(Guid CatechismClassEucharisticYouthId, AttendanceStatus Status)> attendances,
        Guid catechistId,
        CancellationToken cancellationToken)
    {
        CatechismSessionEntity? entity = await _context.CatechismSessions.FirstOrDefaultAsync(y => y.Id == sessionId, cancellationToken);
        if (entity == null)
        {
            throw new Exception(DomainErrors.ValidationNotFound);
        }

        CatechismSession domain = entity.ToDomain();
        domain.MarkAttendances(attendances, catechistId);
        domain.UpdateEntity(entity);
    }

    #endregion
}