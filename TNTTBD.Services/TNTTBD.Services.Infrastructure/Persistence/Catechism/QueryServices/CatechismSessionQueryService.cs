using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Application.CatechismSessions.DTOs;
using TNTTBD.Services.Application.CatechismSessions.Interfaces;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

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
            .Select(x => x.ToDTO())
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<CatechismSessionDto>?> GetSessionsByClassAsync(Guid classId, CancellationToken cancellationToken)
    {
        return await _context.CatechismSessions
            .Where(c => c.CatechismClassId == classId)
            .Select(c => c.ToDTO())
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<CatechismSessionDto>> GetAllAsync(Guid? classId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var query = _context.CatechismSessions.AsNoTracking();

        if (classId.HasValue)
            query = query.Where(s => s.CatechismClassId == classId.Value);

        int total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(s => s.DateTime)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(s => s.ToDTO())
            .ToListAsync(cancellationToken);

        return new PaginatedResult<CatechismSessionDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize
        };
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<CatechismSessionAttendanceDto>> GetAttendancesBySessionAsync(Guid sessionId, int page, int pageSize, CancellationToken cancellationToken)
    {
        var query = _context.CatechismSessionAttendances
            .AsNoTracking()
            .Where(a => a.CatechismSessionId == sessionId);

        int total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(a => a.MarkedTime)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(a => new CatechismSessionAttendanceDto
            {
                Id = a.Id,
                CatechismSessionId = a.CatechismSessionId,
                CatechismClassEucharisticYouthId = a.CatechismClassEucharisticYouthId,
                AttendanceStatus = a.AttendanceStatus,
                MarkedTime = a.MarkedTime,
                MarkedByCatechistId = a.MarkedByCatechistId
            })
            .ToListAsync(cancellationToken);

        return new PaginatedResult<CatechismSessionAttendanceDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize
        };
    }
}