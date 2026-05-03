using TNTTBD.Services.Application.CatechismSessions.DTOs;

namespace TNTTBD.Services.Application.CatechismSessions.Interfaces;

public interface ICatechismSessionQueryService
{
    /// <summary>
    /// Get class by id
    /// </summary>
    Task<CatechismSessionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Get list of class
    /// </summary>
    /// <param name="yearId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<CatechismSessionDto>?> GetSessionsByClassAsync(Guid classId, CancellationToken cancellationToken);
}