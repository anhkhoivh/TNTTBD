using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Domain.Catechism.JoinModels;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.Common;
using TNTTBD.Services.Domain.Enums;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Repositories;

public class CatechismClassCatechistRepository : ICatechismClassCatechistRepository
{
    private readonly CatechismDbContext _context;

    public CatechismClassCatechistRepository(CatechismDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(CatechismClassCatechist domain, CancellationToken cancellationToken)
    {
        CatechismClassCatechistEntity entity = domain.ToEntity();
        await _context.CatechismClassCatechists.AddAsync(entity, cancellationToken);
    }

    public async Task Update(Guid id, CatechismClassCatechistRole role, string? note, DateTimeOffset? leftAt, CancellationToken cancellationToken)
    {
        var query = _context.CatechismClassCatechists.Where(e => e.Id == id);

        await query.ExecuteUpdateAsync(e => e
            .SetProperty(x => x.Role, role)
            .SetProperty(x => x.Note, note)
            .SetProperty(x => x.LeftAt, leftAt), cancellationToken);
    }

    public async Task Remove(Guid id, CancellationToken cancellationToken)
    {
        CatechismClassCatechistEntity? entity = await _context.CatechismClassCatechists
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken)
            ?? throw new DomainException(DomainErrors.ValidationNotFound);

        _context.CatechismClassCatechists.Remove(entity);
    }

    public async Task<bool> HasActiveMainCatechistAsync(Guid classId, CancellationToken cancellationToken)
    {
        return await _context.CatechismClassCatechists
            .AnyAsync(e => e.CatechismClassId == classId
                && e.Role == CatechismClassCatechistRole.Main
                && e.Status == EnrollmentStatus.Active, cancellationToken);
    }
}
