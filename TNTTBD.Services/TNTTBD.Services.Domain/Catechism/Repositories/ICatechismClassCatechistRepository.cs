using TNTTBD.Services.Domain.Catechism.JoinModels;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Domain.Catechism.Repositories;

public interface ICatechismClassCatechistRepository
{
    Task AddAsync(CatechismClassCatechist domain, CancellationToken cancellationToken);
    Task Remove(Guid id, CancellationToken cancellationToken);
    Task Update(Guid id, CatechismClassCatechistRole role, string? note, DateTimeOffset? leftAt, CancellationToken cancellationToken);
    Task<bool> HasActiveMainCatechistAsync(Guid classId, CancellationToken cancellationToken);
}
