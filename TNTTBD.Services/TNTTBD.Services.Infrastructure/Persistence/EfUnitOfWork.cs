using Microsoft.EntityFrameworkCore.Storage;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Infrastructure.Persistence.Catechism;

namespace TNTTBD.Services.Infrastructure.Persistence;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly CatechismDbContext _context;
    private IDbContextTransaction _transaction = null!;

    public EfUnitOfWork(CatechismDbContext context)
    {
        _context = context;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken)
        => _context.SaveChangesAsync(cancellationToken);

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        await _context.SaveChangesAsync();
        await _transaction.CommitAsync();
    }

    public async Task RollbackTransactionAsync()
    {
        await _transaction.RollbackAsync();
    }
}