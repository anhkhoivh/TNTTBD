namespace TNTTBD.Services.Application.Auth.Interfaces;

public interface IAuthorizationContext
{
    Guid? GetCurrentUserId();
    Task<Guid?> GetCurrentCatechistIdAsync(CancellationToken cancellationToken);
    Task<bool> IsAdminAsync(CancellationToken cancellationToken);
    Task<bool> IsCatechistAsync(CancellationToken cancellationToken);
    Task<bool> IsAdminOrCatechistAsync(CancellationToken cancellationToken);
    Task<bool> CanAccessClassAsync(Guid classId, CancellationToken cancellationToken);
    Task<bool> CanAccessYouthAsync(Guid youthId, CancellationToken cancellationToken);
    Task<bool> CanAccessCatechistAsync(Guid catechistId, CancellationToken cancellationToken);
    Task<bool> CanWriteYearsAsync(CancellationToken cancellationToken);
    Task<bool> CanWriteClassCatechistAsync(CancellationToken cancellationToken);
}
