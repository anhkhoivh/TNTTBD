using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly CatechismDbContext _context;

    public ContactRepository(CatechismDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Contact contact, CancellationToken cancellationToken)
    {
        var entity = contact.ToEntity();
        await _context.Contacts.AddAsync(entity, cancellationToken);
    }
}
