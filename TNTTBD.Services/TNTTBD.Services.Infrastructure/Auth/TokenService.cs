using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TNTTBD.Services.Application.Auth.DTOs;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Infrastructure.Persistence.Catechism;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Auth;

public class TokenService : ITokenService
{
    private readonly CatechismDbContext _context;
    private readonly IConfiguration _configuration;

    public TokenService(CatechismDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public string GenerateAccessToken(UserDto user)
    {
        var secret = _configuration["Jwt:Secret"] ?? throw new InvalidOperationException("JWT Secret not configured.");
        var issuer = _configuration["Jwt:Issuer"] ?? "tnttbd";
        var audience = _configuration["Jwt:Audience"] ?? "tnttbd-ui";
        var minutes = int.Parse(_configuration["Jwt:AccessTokenMinutes"] ?? "60");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.UserRole.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(minutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    public async Task<bool> ValidateRefreshTokenAsync(Guid userId, string refreshToken, CancellationToken cancellationToken)
    {
        var tokenHash = HashToken(refreshToken);
        var entity = await _context.RefreshTokens
            .FirstOrDefaultAsync(t => t.UserId == userId && t.TokenHash == tokenHash && !t.IsRevoked, cancellationToken);

        if (entity == null || entity.ExpiresAt < DateTimeOffset.UtcNow)
            return false;

        // Revoke used token (rotation)
        entity.IsRevoked = true;
        return true;
    }

    public async Task SaveRefreshTokenAsync(Guid userId, string refreshToken, CancellationToken cancellationToken)
    {
        var days = int.Parse(_configuration["Jwt:RefreshTokenDays"] ?? "7");

        var entity = new RefreshTokenEntity
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            TokenHash = HashToken(refreshToken),
            ExpiresAt = DateTimeOffset.UtcNow.AddDays(days),
            CreatedAt = DateTimeOffset.UtcNow,
            IsRevoked = false
        };

        await _context.RefreshTokens.AddAsync(entity, cancellationToken);
    }

    public async Task RevokeRefreshTokenAsync(Guid userId, CancellationToken cancellationToken)
    {
        await _context.RefreshTokens
            .Where(t => t.UserId == userId && !t.IsRevoked)
            .ExecuteUpdateAsync(s => s.SetProperty(t => t.IsRevoked, true), cancellationToken);
    }

    private static string HashToken(string token)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
        return Convert.ToBase64String(bytes);
    }
}
