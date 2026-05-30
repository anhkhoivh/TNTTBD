using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Application.EucharisticYouths.DTOs;
using TNTTBD.Services.Application.EucharisticYouths.Interfaces;
using TNTTBD.Services.Infrastructure.Persistence.Catechism;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.QueryServices;

public class EucharisticYouthContactQueryService : IEucharisticYouthContactQueryService
{
    private readonly CatechismDbContext _context;

    public EucharisticYouthContactQueryService(CatechismDbContext context)
    {
        _context = context;
    }

    public async Task<List<EucharisticYouthContactDto>> GetByYouthIdAsync(Guid youthId, CancellationToken cancellationToken)
    {
        return await (
            from yc in _context.EucharisticYouthContacts.AsNoTracking()
            join c in _context.Contacts.AsNoTracking() on yc.ContactId equals c.Id
            where yc.EucharisticYouthId == youthId
            select new EucharisticYouthContactDto
            {
                Id = yc.Id,
                EucharisticYouthId = yc.EucharisticYouthId,
                ContactId = yc.ContactId,
                ContactRelationType = yc.ContactRelationType,
                Contact = new ContactInfoDto
                {
                    Id = c.Id,
                    GivenName = c.FullName.GivenName,
                    FamilyName = c.FullName.FamilyName,
                    PhoneNumber = c.PhoneNumber,
                    Email = c.Email
                }
            }
        ).ToListAsync(cancellationToken);
    }
}
