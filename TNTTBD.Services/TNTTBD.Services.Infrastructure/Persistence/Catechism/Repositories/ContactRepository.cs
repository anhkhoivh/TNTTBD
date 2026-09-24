using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.Common;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;
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

    public async Task<Contact?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        ContactEntity? entity = await _context.Contacts
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        return entity?.ToDomain();
    }

    public async Task UpdateAsync(Contact contact, CancellationToken cancellationToken)
    {
        ContactEntity entity = await _context.Contacts
            .FirstOrDefaultAsync(c => c.Id == contact.Id, cancellationToken)
            ?? throw new DomainException(DomainErrors.ValidationNotFound);

        contact.UpdateEntity(entity);
    }

    public async Task RemoveAsync(Guid id, CancellationToken cancellationToken)
    {
        ContactEntity entity = await _context.Contacts
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken)
            ?? throw new DomainException(DomainErrors.ValidationNotFound);

        bool hasUser = entity.UserId != null
            || await _context.Users.AnyAsync(u => u.ContactId == id, cancellationToken);
        if (hasUser)
        {
            throw new DomainException(DomainErrors.ContactHasUserAccount);
        }

        _context.Contacts.Remove(entity);
    }
}
