using TNTTBD.Services.Domain.Catechism.Members;

namespace TNTTBD.Services.Domain.Catechism.Repositories;

public interface IContactRepository
{
    Task AddAsync(Contact contact, CancellationToken cancellationToken);
}
