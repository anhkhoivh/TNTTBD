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
    Task<Guid?> GetCurrentContactIdAsync(CancellationToken cancellationToken);
    Task<bool> CanAccessContactAsync(Guid contactId, CancellationToken cancellationToken);
    Task<bool> CanReadYouthContactsAsync(Guid youthId, CancellationToken cancellationToken);
    /// <summary>
    /// Read access to a youth's profile and photos: Admin, any catechist, the youth, linked contacts.
    /// </summary>
    Task<bool> CanViewYouthAsync(Guid youthId, CancellationToken cancellationToken);
    /// <summary>
    /// Read access to a catechist's profile and photos: Admin and any catechist.
    /// </summary>
    Task<bool> CanViewCatechistAsync(Guid catechistId, CancellationToken cancellationToken);
    Task<bool> CanManageCatechistPhotosAsync(Guid catechistId, CancellationToken cancellationToken);
    Task<bool> CanManageYouthPhotosAsync(Guid youthId, CancellationToken cancellationToken);
    Task<bool> CanWriteYearsAsync(CancellationToken cancellationToken);
    Task<bool> CanWriteClassCatechistAsync(CancellationToken cancellationToken);
}
