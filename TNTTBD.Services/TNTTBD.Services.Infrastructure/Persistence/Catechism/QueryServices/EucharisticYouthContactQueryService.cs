using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Application.EucharisticYouths.DTOs;
using TNTTBD.Services.Application.EucharisticYouths.Interfaces;

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
        return await  _context.EucharisticYouthContacts
            .AsNoTracking()
            .Where(c => c.EucharisticYouthId == youthId)
            .Select(yc => new EucharisticYouthContactDto
            {
                Id = yc.Id,
                EucharisticYouthId = yc.EucharisticYouthId,
                ContactId = yc.ContactId,
                ContactRelationType = yc.ContactRelationType,
                Contact = new ContactInfoDto
                {
                    Id = yc.Contact.Id,
                    FullName = yc.Contact.FullName,
                    PhoneNumber = yc.Contact.PhoneNumber,
                    Email = yc.Contact.Email
                }
            })
            .ToListAsync(cancellationToken);
    }
}
