using TNTTBD.Services.Application.Auth.DTOs;

namespace TNTTBD.Services.Application.Auth.Interfaces;

public interface IOAuthService
{
    string GetAuthorizationUrl(string provider, string redirectUri, string state);
    Task<OAuthUserInfo> ExchangeCodeAsync(string provider, string code, string redirectUri, CancellationToken cancellationToken);
}
