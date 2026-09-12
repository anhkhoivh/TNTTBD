using MediatR;
using TNTTBD.Services.Application.Auth.Commands;
using TNTTBD.Services.Application.Auth.DTOs;
using TNTTBD.Services.Application.Auth.Interfaces;

namespace TNTTBD.Services.Application.Auth.Handlers;

public class SsoCallbackHandler : IRequestHandler<SsoCallbackCommand, LoginResponse>
{
    private readonly IOAuthService _oAuthService;
    private readonly IExternalLoginRepository _externalLoginRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public SsoCallbackHandler(
        IOAuthService oAuthService,
        IExternalLoginRepository externalLoginRepository,
        IUserRepository userRepository,
        ITokenService tokenService)
    {
        _oAuthService = oAuthService;
        _externalLoginRepository = externalLoginRepository;
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<LoginResponse> Handle(SsoCallbackCommand command, CancellationToken cancellationToken)
    {
        // Exchange authorization code for user info
        var userInfo = await _oAuthService.ExchangeCodeAsync(
            command.Provider, command.Code, command.RedirectUri, cancellationToken);

        // Try to find user by existing external login link
        var userId = await _externalLoginRepository.FindUserIdByExternalLoginAsync(
            userInfo.Provider, userInfo.SubjectId, cancellationToken);

        UserDto? user;

        if (userId.HasValue)
        {
            // Fast path: external login already linked
            user = await _userRepository.GetByIdAsync(userId.Value, cancellationToken);
        }
        else
        {
            // Try to find user by email and auto-link
            user = await _userRepository.GetByIdentifierAsync(userInfo.Email, cancellationToken);

            if (user == null)
            {
                throw new UnauthorizedAccessException(
                    "No account found for this email. Contact your administrator.");
            }

            // Auto-link the external login to this user
            await _externalLoginRepository.LinkExternalLoginAsync(
                user.Id, userInfo.Provider, userInfo.SubjectId, cancellationToken);
        }

        if (user == null || !user.IsActive)
        {
            throw new UnauthorizedAccessException("Account is deactivated or not found.");
        }

        // Issue JWT + refresh token
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
