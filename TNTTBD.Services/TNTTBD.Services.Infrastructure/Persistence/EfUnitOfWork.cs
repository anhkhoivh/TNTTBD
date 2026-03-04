using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Infrastructure.Persistence.Catechism;

namespace TNTTBD.Services.Infrastructure.Persistence;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly CatechismDbContext _context;

    public EfUnitOfWork(CatechismDbContext context)
    {
        _context = context;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
        => _context.SaveChangesAsync(cancellationToken);
}