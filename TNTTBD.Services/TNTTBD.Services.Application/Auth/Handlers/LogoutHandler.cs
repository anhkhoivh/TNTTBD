using MediatR;
using TNTTBD.Services.Application.Auth.Commands;
using TNTTBD.Services.Application.Auth.Interfaces;

namespace TNTTBD.Services.Application.Auth.Handlers;

public class LogoutHandler : IRequestHandler<LogoutCommand>
{
    private readonly ITokenService _tokenService;

    public LogoutHandler(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public async Task Handle(LogoutCommand command, CancellationToken cancellationToken)
    {
        await _tokenService.RevokeRefreshTokenAsync(command.UserId, cancellationToken);
    }
}
