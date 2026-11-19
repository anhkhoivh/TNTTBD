using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Commands;
using TNTTBD.Services.Application.Auth.DTOs;
using TNTTBD.Services.Application.Auth.Interfaces;

namespace TNTTBD.Services.Application.Auth.Handlers;

public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public LoginHandler(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<LoginResponse> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        bool valid = await _userRepository.ValidatePasswordAsync(command.Identifier, command.Password, cancellationToken);
        if (!valid)
            throw new UnauthorizedAccessException("Invalid email or password.");

        var user = await _userRepository.GetByIdentifierAsync(command.Identifier, cancellationToken)
            ?? throw new UnauthorizedAccessException("User not found.");

        if (!user.IsActive)
            throw new UnauthorizedAccessException("Account is deactivated.");

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
