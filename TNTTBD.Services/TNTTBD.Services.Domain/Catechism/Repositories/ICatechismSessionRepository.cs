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

    /// <summary>
    /// Mark attendance list
    /// </summary>
    /// <param name="sessionId"></param>
    /// <param name="attendances"></param>
    /// <param name="catechistId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task MarkAttendanceList(Guid sessionId, 
        IEnumerable<(Guid CatechismClassEucharisticYouthId, AttendanceStatus Status)> attendances,
        Guid catechistId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Mark attendance
    /// </summary>
    /// <param name="sessionId"></param>
    /// <param name="catechismClassEucharisticYouthId"></param>
    /// <param name="status"></param>
    /// <param name="catechistId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task MarkAttendance(Guid sessionId,
        Guid catechismClassEucharisticYouthId, 
        AttendanceStatus status,
        Guid catechistId,
        CancellationToken cancellationToken);
}