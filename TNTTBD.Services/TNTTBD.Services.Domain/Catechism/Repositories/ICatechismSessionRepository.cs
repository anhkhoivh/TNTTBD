using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Domain.Catechism.Repositories;

public interface ICatechismSessionRepository
{
    /// <summary>
    /// Add session
    /// </summary>
    Task AddAsync(CatechismSession catechismSession, CancellationToken cancellationToken);

    /// <summary>
    /// Update session
    /// </summary>
    Task Update(Guid id, DateTimeOffset dateTime, string title, CancellationToken cancellationToken);

    /// <summary>
    /// Remove session
    /// </summary>
    Task Remove(Guid id, CancellationToken cancellationToken);

    Task MarkAttendanceList(Guid sessionId, 
        IEnumerable<(Guid CatechismClassEucharisticYouthId, AttendanceStatus Status)> attendances,
        Guid catechistId,
        CancellationToken cancellationToken);
}