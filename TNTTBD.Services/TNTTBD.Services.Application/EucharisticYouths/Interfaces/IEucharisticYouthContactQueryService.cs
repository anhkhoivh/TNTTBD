using TNTTBD.Services.Application.EucharisticYouths.DTOs;

namespace TNTTBD.Services.Application.EucharisticYouths.Interfaces;

public interface IEucharisticYouthContactQueryService
{
    Task<List<EucharisticYouthContactDto>> GetByYouthIdAsync(Guid youthId, CancellationToken cancellationToken);
}
