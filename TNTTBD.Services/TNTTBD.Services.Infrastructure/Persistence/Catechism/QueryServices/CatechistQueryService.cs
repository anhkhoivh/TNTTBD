using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Catechists.DTOs;
using TNTTBD.Services.Application.Catechists.Interfaces;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Domain.ValueObjects;

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
            .Select(entity => new CatechistDto
            {
                Id = entity.Id,
                FullName = entity.FullName,
                DateOfBirth = entity.DateOfBirth,
                IsMale = entity.IsMale,
                Address = entity.Address,
                PhoneNumber = entity.PhoneNumber,

                Photos = entity.Photos.Select(e => new PhotoReferenceDto
                {
                    Purpose = e.Purpose.ToString(),
                    ContentType = e.ContentType,
                    UploadedBy = e.UploadedBy,
                    Url = storage.GetFileUrl(e.StorageKey)
                }).ToList(),

                PatronSaints = entity.PatronSaints.Select(p => new PatronSaintDto
                {
                    SaintName = p.SaintName,
                    Day = p.Day,
                    Month = p.Month
                }).ToList()
            })
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<PhotoReferenceDto>?> GetPhotosByCatechistId(Guid catechistId, IFileStorage storage, CancellationToken cancellationToken)
    {
        return await _context.CatechistPhotos
            .Where(c => c.CatechistId == catechistId)
            .Select(e => new PhotoReferenceDto
            {
                Purpose = e.Purpose.ToString(),
                ContentType = e.ContentType,
                UploadedBy = e.UploadedBy,
                Url = storage.GetFileUrl(e.StorageKey)
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<CatechistDto>> GetAllAsync(int page, int pageSize, IFileStorage storage, CancellationToken cancellationToken)
    {
        var query = _context.Catechists.AsNoTracking();

        int total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(c => c.FullName.GivenName)
            .ThenBy(c => c.FullName.FamilyName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(entity => new CatechistDto
            {
                Id = entity.Id,
                FullName = entity.FullName,
                DateOfBirth = entity.DateOfBirth,
                IsMale = entity.IsMale,
                Address = entity.Address,
                PhoneNumber = entity.PhoneNumber,

                Photos = entity.Photos.Select(e => new PhotoReferenceDto
                {
                    Purpose = e.Purpose.ToString(),
                    ContentType = e.ContentType,
                    UploadedBy = e.UploadedBy,
                    Url = storage.GetFileUrl(e.StorageKey)
                }).ToList(),

                PatronSaints = entity.PatronSaints.Select(p => new PatronSaintDto
                {
                    SaintName = p.SaintName,
                    Day = p.Day,
                    Month = p.Month
                }).ToList()
            })
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