using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Application.CatechismSessions.DTOs;
using TNTTBD.Services.Application.CatechismSessions.Interfaces;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Domain.Enums;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.QueryServices;

public class CatechismSessionQueryService : ICatechismSessionQueryService
{
    private readonly CatechismDbContext _context;

    public CatechismSessionQueryService(CatechismDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<CatechismSessionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.CatechismSessions
            .Where(x => x.Id == id)
            .Select(entity => new CatechismSessionDto
            {
                Id = entity.Id,
                Title = entity.Title,
                CatechismClassId = entity.CatechismClassId,
                DateTime = entity.DateTime
            })
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<CatechismSessionDto>?> GetSessionsByClassAsync(Guid classId, CancellationToken cancellationToken)
    {
        return await _context.CatechismSessions
            .Where(c => c.CatechismClassId == classId)
            .Select(entity => new CatechismSessionDto
            {
                Id = entity.Id,
                Title = entity.Title,
                CatechismClassId = entity.CatechismClassId,
                DateTime = entity.DateTime
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<CatechismSessionDto>> GetAllAsync(Guid? classId, int page, int? pageSize, CancellationToken cancellationToken)
    {
        var query = _context.CatechismSessions.AsNoTracking();

        if (classId.HasValue)
            query = query.Where(s => s.CatechismClassId == classId.Value);

        int total = await query.CountAsync(cancellationToken);

        if (pageSize.HasValue)
        {
            query = query.Skip((page - 1) * pageSize.Value).Take(pageSize.Value);
        }

        var items = await query
            .OrderByDescending(s => s.DateTime)
            .Select(entity => new CatechismSessionDto
            {
                Id = entity.Id,
                Title = entity.Title,
                CatechismClassId = entity.CatechismClassId,
                DateTime = entity.DateTime
            })
            .ToListAsync(cancellationToken);

        return new PaginatedResult<CatechismSessionDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize ?? total
        };
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<CatechismSessionAttendanceDto>> GetAttendancesBySessionAsync(Guid sessionId, int page, int? pageSize, CancellationToken cancellationToken)
    {
        var query = _context.CatechismSessionAttendances
            .AsNoTracking()
            .Where(a => a.CatechismSessionId == sessionId);

        int total = await query.CountAsync(cancellationToken);

        if (pageSize.HasValue)
        {
            query = query.Skip((page - 1) * pageSize.Value).Take(pageSize.Value);
        }

        var items = await query
            .OrderBy(a => a.MarkedTime)
            .Select(a => new CatechismSessionAttendanceDto
            {
                Id = a.Id,
                CatechismClassEucharisticYouthId = a.CatechismClassEucharisticYouthId,
                YouthCode = a.CatechismClassEucharisticYouth.EucharisticYouth.Code,
                YouthFullName = a.CatechismClassEucharisticYouth.EucharisticYouth.FullName,
                AttendanceStatus = a.AttendanceStatus,
            })
            .ToListAsync(cancellationToken);

        return new PaginatedResult<CatechismSessionAttendanceDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize ?? total
        };
    }

    /// <inheritdoc />
    public async Task<List<CatechismSessionAttendanceDto>> GetAttendanceSheetAsync(Guid sessionId, CancellationToken cancellationToken)
    {
        CatechismSessionEntity? session = await _context.CatechismSessions
            .AsNoTracking()
            .Where(s => s.Id == sessionId)
            .FirstOrDefaultAsync(cancellationToken);

        if (session == null)
        {
            return [];
        }

        var existingAttendances = await _context.CatechismSessionAttendances
            .AsNoTracking()
            .Where(a => a.CatechismSessionId == sessionId)
            .Select(a => new { a.Id, a.CatechismClassEucharisticYouthId, a.AttendanceStatus })
            .ToListAsync(cancellationToken);

        var attendanceLookup = existingAttendances.ToDictionary(a => a.CatechismClassEucharisticYouthId);

        var enrollments = await _context.CatechismClassEucharisticYouths
            .AsNoTracking()
            .Where(cy => cy.CatechismClassId == session.CatechismClassId && cy.Status == EnrollmentStatus.Active)
            .OrderBy(cy => cy.EucharisticYouth.FullName.GivenName).ThenBy(cy => cy.EucharisticYouth.FullName.FamilyName)
            .Select(cy => new { cy.Id, cy.EucharisticYouth.Code, cy.EucharisticYouth.FullName })
            .ToListAsync(cancellationToken);

        return enrollments.Select(e =>
        {
            attendanceLookup.TryGetValue(e.Id, out var a);
            return new CatechismSessionAttendanceDto
            {
                Id = a?.Id,
                CatechismClassEucharisticYouthId = e.Id,
                YouthCode = e.Code,
                YouthFullName = e.FullName,
                AttendanceStatus = a?.AttendanceStatus ?? AttendanceStatus.None,
            };
        }).ToList();
    }
}