using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Catechists.DTOs;
using TNTTBD.Services.Application.Catechists.Interfaces;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Domain.Enums;
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
                    PhotoId = e.Id,
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
                PhotoId = e.Id,
                Purpose = e.Purpose.ToString(),
                ContentType = e.ContentType,
                UploadedBy = e.UploadedBy,
                Url = storage.GetFileUrl(e.StorageKey)
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<CatechistDto>> GetAllAsync(int page, int? pageSize, string? search, Guid? pinnedCatechistId, IFileStorage storage, CancellationToken cancellationToken)
    {
        var query = _context.Catechists.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            query = query.Where(c =>
                c.FullName.GivenName.ToLower().Contains(s) ||
                (c.FullName.FamilyName != null && c.FullName.FamilyName.ToLower().Contains(s)) ||
                (c.FullName.MiddleName != null && c.FullName.MiddleName.ToLower().Contains(s)) ||
                (c.FullName.ChristName != null && c.FullName.ChristName.ToLower().Contains(s)));
        }

        int total = await query.CountAsync(cancellationToken);

        query = query
            .OrderByDescending(c => pinnedCatechistId.HasValue && c.Id == pinnedCatechistId.Value)
            .ThenBy(c => c.FullName.GivenName)
            .ThenBy(c => c.FullName.FamilyName)
            .ThenBy(c => c.Id);

        if (pageSize.HasValue)
        {
            query = query.Skip((page - 1) * pageSize.Value).Take(pageSize.Value);
        }

        var items = await query
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
                    PhotoId = e.Id,
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
            PageSize = pageSize ?? total
        };
    }

    /// <inheritdoc />
    public async Task<CatechistCurrentClassDto?> GetCurrentClassAsync(Guid catechistId, CancellationToken cancellationToken)
    {
        var latestYear = await _context.CatechismYears
            .AsNoTracking()
            .OrderByDescending(y => y.StartDate)
            .Select(y => new { y.Id, y.StartDate, y.EndDate })
            .FirstOrDefaultAsync(cancellationToken);

        if (latestYear == null) return null;

        return await _context.CatechismClassCatechists
            .AsNoTracking()
            .Where(cc => cc.CatechistId == catechistId
                && cc.Status == EnrollmentStatus.Active
                && cc.CatechismClass.CatechismYearId == latestYear.Id)
            .Select(cc => new CatechistCurrentClassDto
            {
                ClassId = cc.CatechismClassId,
                ClassName = cc.CatechismClass.ClassName,
                CatechismYearId = latestYear.Id,
                CatechismYearTitle = $"{latestYear.StartDate.Year}-{latestYear.EndDate.Year}",
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    #endregion
}