using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Repositories;

public class EucharisticYouthRunningNumberRepository : IEucharisticYouthRunningNumberRepository
{
    private readonly CatechismDbContext _context;

    public EucharisticYouthRunningNumberRepository(CatechismDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(EucharisticYouthRunningNumber runningNumber, CancellationToken cancellationToken)
    {
        await _context.EucharisticYouthRunningNumbers.AddAsync(runningNumber.ToEntity(), cancellationToken);
    }

    public async Task<EucharisticYouthRunningNumber?> GetByYearAsync(int year, CancellationToken cancellationToken)
    {
        EucharisticYouthRunningNumberEntity? entity = await _context.EucharisticYouthRunningNumbers
            .FirstOrDefaultAsync(x => x.Year == year, cancellationToken);

        return entity?.ToDomain();
    }

    /// <inheritdoc />
    public async Task<int> IncreaseRunningNumber(int year, CancellationToken cancellationToken)
    {
        EucharisticYouthRunningNumberEntity? entity = await _context.EucharisticYouthRunningNumbers
            .FirstOrDefaultAsync(x => x.Year == year, cancellationToken);
        if (entity == null)
        {
            EucharisticYouthRunningNumber newRunningNumber = new (Guid.NewGuid(), year, 1);
            await AddAsync(newRunningNumber, cancellationToken);
            return newRunningNumber.CurrentNumber;
        }
        
        EucharisticYouthRunningNumber domain = entity.ToDomain();
        domain.IncreaseNumber();
        domain.UpdateEntity(entity);
        return domain.CurrentNumber;
    }
}