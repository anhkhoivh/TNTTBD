using MediatR;
using TNTTBD.Services.Application.Auth.Commands;
using TNTTBD.Services.Application.Auth.DTOs;
using TNTTBD.Services.Application.Auth.Interfaces;

namespace TNTTBD.Services.Application.Auth.Handlers;

public class RefreshTokenHandler : IRequestHandler<RefreshTokenCommand, LoginResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public RefreshTokenHandler(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<LoginResponse> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        bool valid = await _tokenService.ValidateRefreshTokenAsync(command.UserId, command.RefreshToken, cancellationToken);
        if (!valid)
            throw new UnauthorizedAccessException("Invalid or expired refresh token.");

        var user = await _userRepository.GetByIdAsync(command.UserId, cancellationToken)
            ?? throw new UnauthorizedAccessException("User not found.");

        string token = _tokenService.GenerateAccessToken(user);
        string refreshToken = _tokenService.GenerateRefreshToken();
        await _tokenService.SaveRefreshTokenAsync(user.Id, refreshToken, cancellationToken);

        return new LoginResponse
        {
            Token = token,
            RefreshToken = refreshToken,
            User = user
        };
    }
}
