using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Domain.Catechism.Repositories;

public interface ICatechismYearRepository
{
    /// <summary>
    /// Add new year
    /// </summary>
    Task AddAsync(CatechismYear catechismYear, CancellationToken cancellationToken);

    /// <summary>
    /// Update year info
    /// </summary>
    /// <param name="id"></param>
    /// <param name="startDate"></param>
    /// <param name="endDate"></param>
    /// <param name="theme"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task Update(Guid id, DateOnly startDate, DateOnly endDate, string? theme, CancellationToken cancellationToken);

    /// <summary>
    /// Remove year
    /// </summary>
    Task Remove(Guid id, CancellationToken cancellationToken);
}