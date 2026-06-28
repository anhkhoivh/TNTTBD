using TNTTBD.Services.Domain.Catechism.JoinModels;

namespace TNTTBD.Services.Domain.Catechism.Repositories;

public interface IEucharisticYouthContactRepository
{
    Task AddAsync(EucharisticYouthContact link, CancellationToken cancellationToken);
}
