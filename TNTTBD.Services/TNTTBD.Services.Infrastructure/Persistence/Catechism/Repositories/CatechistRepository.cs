using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.ValueObjects;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Repositories;

public class CatechistRepository : ICatechistRepository
{
    private readonly CatechismDbContext _context;

    public CatechistRepository(CatechismDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        Catechist catechist,
        CancellationToken cancellationToken)
    {
        CatechistEntity entity = catechist.ToEntity();
        await _context.Catechists.AddAsync(entity, cancellationToken);
    }

    public async Task<Catechist?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        CatechistEntity? entity = await _context.Catechists
            .Include(x => x.Photos)
            .Include(x => x.PatronSaints)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity?.ToDomain();
    }

    /// <inheritdoc />
    public async Task AddPhoto(Guid id, PhotoReference photoReference, CancellationToken cancellationToken)
    {
        CatechistEntity? catechistEntity = await _context.Catechists
            .Include(y => y.Photos)
            .FirstOrDefaultAsync(y => y.Id == id, cancellationToken);
        if (catechistEntity == null)
        {
            return;
        }
        Catechist domain = catechistEntity.ToDomain();
        domain.AddPhoto(photoReference);
        domain.UpdateEntity(catechistEntity);
    }

    /// <inheritdoc />
    public async Task AddPatronSaint(Guid id, PatronSaint patronSaint, CancellationToken cancellationToken)
    {
        CatechistEntity? catechistEntity = await _context.Catechists
            .Include(y => y.PatronSaints)
            .FirstOrDefaultAsync(y => y.Id == id, cancellationToken);
        if (catechistEntity == null)
        {
            return;
        }
        Catechist domain = catechistEntity.ToDomain();
        domain.AddPatronSaint(patronSaint);
        domain.UpdateEntity(catechistEntity);
    }
}