using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Catechists.DTOs;
using TNTTBD.Services.Application.Catechists.Interfaces;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Domain.Common;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.QueryServices;

public class CatechistQueryService : ICatechistQueryService
{
    #region Fields

    private readonly CatechismDbContext _context;

    #endregion

    #region Constructors

    public CatechistQueryService(CatechismDbContext context)
    {
        _context = context;
    }

    #endregion

    #region Methods

    /// <inheritdoc />
    public async Task<CatechistDto?> GetByIdAsync(Guid id, IFileStorage storage, CancellationToken cancellationToken)
    {
        return await _context.Catechists
            .Where(c => c.Id == id)
            .Select(c => c.ToDTO(storage))
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<PhotoReferenceDto>?> GetPhotosByCatechistId(Guid catechistId, IFileStorage storage, CancellationToken cancellationToken)
    {
        return await _context.CatechistPhotos
            .Where(c => c.CatechistId == catechistId)
            .Select(c => c.ToDTO(storage))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<CatechistDto>> GetAllAsync(int page, int pageSize, IFileStorage storage, CancellationToken cancellationToken)
    {
        var query = _context.Catechists.AsNoTracking();

        int total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(c => c.FullName.FamilyName)
            .ThenBy(c => c.FullName.GivenName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => c.ToDTO(storage))
            .ToListAsync(cancellationToken);

        return new PaginatedResult<CatechistDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize
        };
    }

    #endregion
}