using TNTTBD.Services.Domain.Catechism.JoinModels;

namespace TNTTBD.Services.Domain.Catechism.Repositories;

public interface IEucharisticYouthContactRepository
{
    Task AddAsync(EucharisticYouthContact link, CancellationToken cancellationToken);
    Task<EucharisticYouthContact?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Guid youthId, Guid contactId, CancellationToken cancellationToken);
}
