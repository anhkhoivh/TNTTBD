namespace TNTTBD.Services.Application.Auth.Commands;

public record RefreshTokenCommand(string RefreshToken, Guid UserId);
