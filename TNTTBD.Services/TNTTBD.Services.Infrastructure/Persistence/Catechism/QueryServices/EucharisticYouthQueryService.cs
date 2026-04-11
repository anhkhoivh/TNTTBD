using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.EucharisticYouths.DTOs;
using TNTTBD.Services.Application.EucharisticYouths.Interfaces;
using TNTTBD.Services.Domain.Common;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.QueryServices;

public class EucharisticYouthQueryService : IEucharisticYouthQueryService
{
    private readonly CatechismDbContext _context;

    public EucharisticYouthQueryService(CatechismDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<EucharisticYouthDto?> GetByIdAsync(Guid id, IFileStorage storage, CancellationToken cancellationToken)
    {
        return await _context.EucharisticYouths
            .Where(y => y.Id == id)
            .Select(y => y.ToDTO(storage))
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken) ?? throw new Exception(DomainErrors.ValidationNotFound);
    }
}