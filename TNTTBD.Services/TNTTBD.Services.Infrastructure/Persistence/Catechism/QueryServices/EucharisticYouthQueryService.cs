using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.EucharisticYouths.DTOs;
using TNTTBD.Services.Application.EucharisticYouths.Interfaces;

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
            .Select(entity => new EucharisticYouthDto
            {
                Id = entity.Id,
                FullName = entity.FullName,
                DateOfBirth = entity.DateOfBirth,
                IsMale = entity.IsMale,
                Address = entity.Address,
                PhoneNumber = entity.PhoneNumber,

                Photos = entity.Photos.Select(p => new PhotoReferenceDto
                {
                    Purpose = p.Purpose.ToString(),
                    ContentType = p.ContentType,
                    UploadedBy = p.UploadedBy,
                    Url = storage.GetFileUrl(p.StorageKey)
                }).ToList(),

                Milestones = entity.SpiritualMilestones.Select(m => new SpiritualMilestoneDto
                {
                    Type = m.Type.ToString(),
                    Date = m.Date,
                    Place = m.Place,
                    Note = m.Note
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
    public async Task<List<PhotoReferenceDto>?> GetPhotosByYouthId(Guid youthId, IFileStorage storage, CancellationToken cancellationToken)
    {
        return await _context.EucharisticYouthPhotos
            .Where(c => c.EucharisticYouthId == youthId)
            .Select(entity => new PhotoReferenceDto
            {
                Purpose = entity.Purpose.ToString(),
                ContentType = entity.ContentType,
                UploadedBy = entity.UploadedBy,
                Url = storage.GetFileUrl(entity.StorageKey)
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<EucharisticYouthDto>> GetAllAsync(string? search, int page, int pageSize, IFileStorage storage, CancellationToken cancellationToken)
    {
        var query = _context.EucharisticYouths.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();
            query = query.Where(y =>
                y.Code.ToLower().Contains(term) ||
                (y.FullName.GivenName != null && y.FullName.GivenName.ToLower().Contains(term)) ||
                (y.FullName.FamilyName != null && y.FullName.FamilyName.ToLower().Contains(term)) ||
                (y.FullName.ChristName != null && y.FullName.ChristName.ToLower().Contains(term)));
        }

        int total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(y => y.FullName.FamilyName)
            .ThenBy(y => y.FullName.GivenName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(entity => new EucharisticYouthDto
            {
                Id = entity.Id,
                FullName = entity.FullName,
                DateOfBirth = entity.DateOfBirth,
                IsMale = entity.IsMale,
                Address = entity.Address,
                PhoneNumber = entity.PhoneNumber,

                Photos = entity.Photos.Select(p => new PhotoReferenceDto
                {
                    Purpose = p.Purpose.ToString(),
                    ContentType = p.ContentType,
                    UploadedBy = p.UploadedBy,
                    Url = storage.GetFileUrl(p.StorageKey)
                }).ToList(),

                Milestones = entity.SpiritualMilestones.Select(m => new SpiritualMilestoneDto
                {
                    Type = m.Type.ToString(),
                    Date = m.Date,
                    Place = m.Place,
                    Note = m.Note
                }).ToList(),

                PatronSaints = entity.PatronSaints.Select(p => new PatronSaintDto
                {
                    SaintName = p.SaintName,
                    Day = p.Day,
                    Month = p.Month
                }).ToList()
            })
            .ToListAsync(cancellationToken);

        return new PaginatedResult<EucharisticYouthDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize
        };
    }
}