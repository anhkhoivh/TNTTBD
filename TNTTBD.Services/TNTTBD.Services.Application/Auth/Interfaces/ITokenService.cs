using TNTTBD.Services.Application.Auth.DTOs;

namespace TNTTBD.Services.Application.Auth.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(UserDto user);
    string GenerateRefreshToken();
    Task<bool> ValidateRefreshTokenAsync(Guid userId, string refreshToken, CancellationToken cancellationToken);
    Task SaveRefreshTokenAsync(Guid userId, string refreshToken, CancellationToken cancellationToken);
    Task RevokeRefreshTokenAsync(Guid userId, CancellationToken cancellationToken);
}
