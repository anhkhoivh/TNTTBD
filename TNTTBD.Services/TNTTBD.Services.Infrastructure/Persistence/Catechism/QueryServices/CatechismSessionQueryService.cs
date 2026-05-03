using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Application.CatechismSessions.DTOs;
using TNTTBD.Services.Application.CatechismSessions.Interfaces;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.QueryServices;

public class CatechismSessionQueryService : ICatechismSessionQueryService
{
    #region Fields

    private readonly CatechismDbContext _context;

    #endregion

    #region Constructors

    public CatechismSessionQueryService(CatechismDbContext context)
    {
        _context = context;
    }

    #endregion

    #region Methods

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

    #endregion
}