using TNTTBD.Services.Application.Auth.DTOs;

namespace TNTTBD.Services.Application.Auth.Interfaces;

public interface IExternalLoginRepository
{
    Task<Guid?> FindUserIdByExternalLoginAsync(string provider, string subjectId, CancellationToken cancellationToken);
    Task LinkExternalLoginAsync(Guid userId, string provider, string subjectId, CancellationToken cancellationToken);
    Task<List<string>> GetLinkedProvidersAsync(Guid userId, CancellationToken cancellationToken);
    Task<bool> UnlinkExternalLoginAsync(Guid userId, string provider, CancellationToken cancellationToken);
}
