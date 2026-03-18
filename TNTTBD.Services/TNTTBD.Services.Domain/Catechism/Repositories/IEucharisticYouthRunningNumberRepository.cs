using TNTTBD.Services.Domain.Catechism.Members;

namespace TNTTBD.Services.Domain.Catechism.Repositories;

public interface IEucharisticYouthRunningNumberRepository
{
    Task AddAsync(EucharisticYouthRunningNumber runningNumber, CancellationToken cancellationToken);

    Task<int> IncreaseRunningNumber(int year, CancellationToken cancellationToken);
}