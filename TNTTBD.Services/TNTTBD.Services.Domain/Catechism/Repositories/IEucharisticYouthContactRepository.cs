using TNTTBD.Services.Domain.Catechism.JoinModels;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Domain.Catechism.Repositories;

public interface IEucharisticYouthContactRepository
{
    Task AddAsync(EucharisticYouthContact link, CancellationToken cancellationToken);
    Task<EucharisticYouthContact?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task RemoveAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Guid youthId, Guid contactId, CancellationToken cancellationToken);
    Task LinkGodparentAsync(Guid youthId, MilestoneType type, Guid? sponsorContactId, CancellationToken cancellationToken);
}
