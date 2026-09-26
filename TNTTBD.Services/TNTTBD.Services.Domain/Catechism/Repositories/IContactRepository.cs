using TNTTBD.Services.Domain.Catechism.Members;

namespace TNTTBD.Services.Domain.Catechism.Repositories;

public interface IContactRepository
{
    Task AddAsync(Contact contact, CancellationToken cancellationToken);
    Task<Contact?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task UpdateAsync(Contact contact, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, CancellationToken cancellationToken);
}
