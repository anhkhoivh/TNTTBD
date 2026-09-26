using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.Contacts.DTOs;
using TNTTBD.Services.Application.Contacts.Interfaces;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.QueryServices;

public class ContactQueryService : IContactQueryService
{
    private readonly CatechismDbContext _context;

    public ContactQueryService(CatechismDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedResult<ContactDto>> GetAllAsync(int page, int? pageSize, string? search, CancellationToken cancellationToken)
    {
        var query = _context.Contacts.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            query = query.Where(c =>
                (c.PhoneNumber != null && c.PhoneNumber.Contains(s)) ||
                (c.Email != null && c.Email.ToLower().Contains(s)) ||
                c.FullName.GivenName.ToLower().Contains(s) ||
                (c.FullName.FamilyName != null && c.FullName.FamilyName.ToLower().Contains(s)) ||
                (c.FullName.ChristName != null && c.FullName.ChristName.ToLower().Contains(s)));
        }

        int total = await query.CountAsync(cancellationToken);

        var ordered = query
            .OrderBy(c => c.FullName.GivenName)
            .ThenBy(c => c.FullName.FamilyName)
            .ThenBy(c => c.Id);

        IQueryable<Entities.ContactEntity> paged = pageSize.HasValue
            ? ordered.Skip((page - 1) * pageSize.Value).Take(pageSize.Value)
            : ordered;

        var items = await paged
            .Select(c => new ContactDto
            {
                Id = c.Id,
                FullName = c.FullName,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
            })
            .ToListAsync(cancellationToken);

        return new PaginatedResult<ContactDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize ?? total,
        };
    }

    public async Task<ContactDetailDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Contacts
            .AsNoTracking()
            .Where(c => c.Id == id)
            .Select(c => new ContactDetailDto
            {
                Id = c.Id,
                FullName = c.FullName,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                Youths = c.EucharisticYouthContacts
                    .Select(yc => new ContactYouthDto
                    {
                        Id = yc.Id,
                        EucharisticYouthId = yc.EucharisticYouthId,
                        Code = yc.EucharisticYouth.Code,
                        FullName = yc.EucharisticYouth.FullName,
                        ContactRelationType = yc.ContactRelationType,
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}
