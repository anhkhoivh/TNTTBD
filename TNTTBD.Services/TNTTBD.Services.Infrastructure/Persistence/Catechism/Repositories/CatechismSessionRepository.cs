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
            throw new DomainException(DomainErrors.ValidationNotFound);
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
            throw new DomainException(DomainErrors.ValidationNotFound);
        }
        _context.CatechismSessions.Remove(entity);
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

        var now = DateTimeOffset.UtcNow;
        var existingDict = entity.EucharisticYouthAttendances.ToDictionary(a => a.CatechismClassEucharisticYouthId);

        foreach (var (youthId, status) in attendances)
        {
            if (existingDict.TryGetValue(youthId, out var existing))
            {
                existing.AttendanceStatus = status;
                existing.MarkedTime = now;
                existing.MarkedByCatechistId = catechistId;
            }
            else
            {
                _context.CatechismSessionAttendances.Add(new CatechismSessionAttendanceEntity
                {
                    Id = Guid.NewGuid(),
                    CatechismSessionId = sessionId,
                    CatechismClassEucharisticYouthId = youthId,
                    AttendanceStatus = status,
                    MarkedTime = now,
                    MarkedByCatechistId = catechistId
                });
            }
        }
    }

    /// <inheritdoc />
    public async Task MarkAttendance(
        Guid sessionId,
        Guid catechismClassEucharisticYouthId, 
        AttendanceStatus status,
        Guid catechistId,
        CancellationToken cancellationToken)
    {
        CatechismSessionEntity? entity = await _context.CatechismSessions.Include(s => s.EucharisticYouthAttendances).FirstOrDefaultAsync(y => y.Id == sessionId, cancellationToken);
        if (entity == null)
        {
            throw new DomainException(DomainErrors.ValidationNotFound);
        }
        var attendanceEntity = entity.EucharisticYouthAttendances.FirstOrDefault(a => a.CatechismClassEucharisticYouthId == catechismClassEucharisticYouthId);
        if (attendanceEntity != null)
        {
            attendanceEntity.AttendanceStatus = status;
            attendanceEntity.MarkedTime = DateTimeOffset.UtcNow;
            attendanceEntity.MarkedByCatechistId = catechistId;
        }
        else
        {
            // Add via DbSet: a pre-set Guid key added through the navigation is treated as Modified (UPDATE of 0 rows).
            _context.CatechismSessionAttendances.Add(new CatechismSessionAttendanceEntity
            {
                Id = Guid.NewGuid(),
                CatechismSessionId = sessionId,
                CatechismClassEucharisticYouthId = catechismClassEucharisticYouthId,
                AttendanceStatus = status,
                MarkedTime = DateTimeOffset.UtcNow,
                MarkedByCatechistId = catechistId
            });
        }
    }

    #endregion
}