using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.EucharisticYouths.DTOs;
using TNTTBD.Services.Application.EucharisticYouths.Interfaces;
using TNTTBD.Services.Domain.Enums;

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
                Code = entity.Code,
                FullName = entity.FullName,
                DateOfBirth = entity.DateOfBirth,
                IsMale = entity.IsMale,
                Address = entity.Address,
                PhoneNumber = entity.PhoneNumber,

                Photos = entity.Photos.Select(p => new PhotoReferenceDto
                {
                    PhotoId = p.Id,
                    Purpose = p.Purpose.ToString(),
                    ContentType = p.ContentType,
                    UploadedBy = p.UploadedBy,
                    Url = storage.GetFileUrl(p.StorageKey)
                }).ToList(),

                Milestones = entity.SpiritualMilestones.Select(m => new SpiritualMilestoneDto(
                    m.Id,
                    m.Type,
                    m.Date,
                    m.Place,
                    m.DocumentCode,
                    m.Note
                )).ToList(),

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
                PhotoId = entity.Id,
                Purpose = entity.Purpose.ToString(),
                ContentType = entity.ContentType,
                UploadedBy = entity.UploadedBy,
                Url = storage.GetFileUrl(entity.StorageKey)
            })
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ProfilePhotoFileDto?> GetProfilePhotoAsync(Guid youthId, CancellationToken cancellationToken)
    {
        return await _context.EucharisticYouthPhotos
            .AsNoTracking()
            .Where(p => p.EucharisticYouthId == youthId && p.Purpose == PhotoPurpose.Profile)
            .OrderByDescending(p => p.UploadedAt)
            .Select(p => new ProfilePhotoFileDto { PhotoId = p.Id, StorageKey = p.StorageKey, ContentType = p.ContentType })
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<EucharisticYouthDto>> GetAllAsync(string? search, Guid? classId, int page, int? pageSize, IFileStorage storage, CancellationToken cancellationToken)
    {
        var query = _context.EucharisticYouths.AsNoTracking();

        if (classId.HasValue)
        {
            query = query.Where(y => _context.CatechismClassEucharisticYouths
                .Any(cy => cy.EucharisticYouthId == y.Id
                    && cy.CatechismClassId == classId.Value
                    && cy.Status == EnrollmentStatus.Active));
        }

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

        query = query
            .OrderBy(y => y.FullName.FamilyName)
            .ThenBy(y => y.FullName.GivenName);

        if (pageSize.HasValue)
        {
            query = query.Skip((page - 1) * pageSize.Value).Take(pageSize.Value);
        }

        var items = await query
            .Select(entity => new EucharisticYouthDto
            {
                Id = entity.Id,
                Code = entity.Code,
                FullName = entity.FullName,
                DateOfBirth = entity.DateOfBirth,
                IsMale = entity.IsMale,
                Address = entity.Address,
                PhoneNumber = entity.PhoneNumber,

                Photos = entity.Photos.Select(p => new PhotoReferenceDto
                {
                    PhotoId = p.Id,
                    Purpose = p.Purpose.ToString(),
                    ContentType = p.ContentType,
                    UploadedBy = p.UploadedBy,
                    Url = storage.GetFileUrl(p.StorageKey)
                }).ToList(),

                Milestones = entity.SpiritualMilestones.Select(m => new SpiritualMilestoneDto(
                    m.Id,
                    m.Type,
                    m.Date,
                    m.Place,
                    m.DocumentCode,
                    m.Note
                )).ToList(),

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
            PageSize = pageSize ?? total
        };
    }
}