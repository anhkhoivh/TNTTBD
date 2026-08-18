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
            var s = search.Trim();
            query = query.Where(c =>
                (c.PhoneNumber != null && c.PhoneNumber.Contains(s)) ||
                (c.Email != null && c.Email.Contains(s)) ||
                c.FullName.GivenName.Contains(s) ||
                (c.FullName.FamilyName != null && c.FullName.FamilyName.Contains(s)));
        }

        int total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(c => c.FullName.GivenName)
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
}
