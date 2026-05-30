using TNTTBD.Services.Domain.Catechism.JoinModels;
using TNTTBD.Services.Domain.Catechism.Repositories;
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
}
