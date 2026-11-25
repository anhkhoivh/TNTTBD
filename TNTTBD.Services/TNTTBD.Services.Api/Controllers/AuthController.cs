using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Commands;
using TNTTBD.Services.Application.Auth.DTOs;
using TNTTBD.Services.Application.Auth.Interfaces;

namespace TNTTBD.Services.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IOAuthService _oAuthService;
    private readonly IExternalLoginRepository _externalLoginRepository;

    public AuthController(
        IUserRepository userRepository,
        IOAuthService oAuthService,
        IExternalLoginRepository externalLoginRepository)
    {
        _userRepository = userRepository;
        _oAuthService = oAuthService;
        _externalLoginRepository = externalLoginRepository;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginCommand command,
        [FromServices] IRequestHandler<LoginCommand, LoginResponse> handler,
        CancellationToken cancellationToken)
    {
        LoginResponse result = await handler.Handle(command, cancellationToken);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(
        [FromBody] RefreshTokenCommand command,
        [FromServices] IRequestHandler<RefreshTokenCommand, LoginResponse> handler,
        CancellationToken cancellationToken)
    {
        LoginResponse result = await handler.Handle(command, cancellationToken);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(
        [FromServices] IRequestHandler<LogoutCommand> handler,
        CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }

        await handler.Handle(new LogoutCommand(Guid.Parse(userId)), cancellationToken);
        return Ok();
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetMe(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null || !Guid.TryParse(userId, out var id))
        {
            return Unauthorized();
        }

        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (user == null || !user.IsActive)
        {
            return Unauthorized();
        }

        return Ok(user);
    }

    [AllowAnonymous]
    [HttpGet("sso/url")]
    public IActionResult GetSsoUrl([FromQuery] string provider, [FromQuery] string redirectUri, [FromQuery] string state)
    {
        if (string.IsNullOrWhiteSpace(provider) || string.IsNullOrWhiteSpace(redirectUri) || string.IsNullOrWhiteSpace(state))
        {
            return BadRequest("provider, redirectUri, and state are required.");
        }

        var url = _oAuthService.GetAuthorizationUrl(provider, redirectUri, state);
        return Ok(new { url });
    }

    [AllowAnonymous]
    [HttpPost("sso-callback")]
    public async Task<IActionResult> SsoCallback(
        [FromBody] SsoCallbackCommand command,
        [FromServices] IRequestHandler<SsoCallbackCommand, LoginResponse> handler,
        CancellationToken cancellationToken)
    {
        LoginResponse result = await handler.Handle(command, cancellationToken);
        return Ok(result);
    }

    [Authorize]
    [HttpGet("linked-accounts")]
    public async Task<IActionResult> GetLinkedAccounts(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null || !Guid.TryParse(userId, out var id))
        {
            return Unauthorized();
        }

        var providers = await _externalLoginRepository.GetLinkedProvidersAsync(id, cancellationToken);
        return Ok(providers);
    }

    [Authorize]
    [HttpDelete("linked-accounts/{provider}")]
    public async Task<IActionResult> UnlinkAccount(string provider, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null || !Guid.TryParse(userId, out var id))
        {
            return Unauthorized();
        }

        // Ensure user retains at least one auth method
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (user == null)
        {
            return Unauthorized();
        }

        var linkedProviders = await _externalLoginRepository.GetLinkedProvidersAsync(id, cancellationToken);
        var hasPassword = await _userRepository.HasPasswordAsync(id, cancellationToken);

        if (!hasPassword && linkedProviders.Count <= 1)
        {
            return BadRequest("Cannot unlink the last authentication method. Set a password first.");
        }

        var removed = await _externalLoginRepository.UnlinkExternalLoginAsync(id, provider, cancellationToken);
        if (!removed)
        {
            return NotFound();
        }

        return Ok();
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(
        [FromBody] ChangeOwnPasswordCommand command,
        [FromServices] IRequestHandler<ChangeOwnPasswordCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.Handle(command, cancellationToken);
        return Ok();
    }
}
