using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Domain.Catechism.JoinModels;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.Common;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Repositories;

public class EucharisticYouthContactRepository : IEucharisticYouthContactRepository
{
    private readonly CatechismDbContext _context;

    public EucharisticYouthContactRepository(CatechismDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(EucharisticYouthContact link, CancellationToken cancellationToken)
    {
        var entity = link.ToEntity();
        await _context.EucharisticYouthContacts.AddAsync(entity, cancellationToken);
    }

    public async Task<EucharisticYouthContact?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        EucharisticYouthContactEntity? entity = await _context.EucharisticYouthContacts
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        return entity?.ToDomain();
    }

    public async Task RemoveAsync(Guid id, CancellationToken cancellationToken)
    {
        EucharisticYouthContactEntity entity = await _context.EucharisticYouthContacts
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken)
            ?? throw new DomainException(DomainErrors.ValidationNotFound);

        _context.EucharisticYouthContacts.Remove(entity);
    }

    public async Task<bool> ExistsAsync(Guid youthId, Guid contactId, CancellationToken cancellationToken)
    {
        return await _context.EucharisticYouthContacts
            .AnyAsync(e => e.EucharisticYouthId == youthId && e.ContactId == contactId, cancellationToken);
    }
}
