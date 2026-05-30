using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TNTTBD.Services.Application.Auth.Commands;
using TNTTBD.Services.Application.Auth.DTOs;
using TNTTBD.Services.Application.Auth.Interfaces;

namespace TNTTBD.Services.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : Controller
{
    private readonly IMediator _mediator;
    private readonly IUserRepository _userRepository;

    public AuthController(IMediator mediator, IUserRepository userRepository)
    {
        _mediator = mediator;
        _userRepository = userRepository;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
    {
        LoginResponse result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        LoginResponse result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }

        await _mediator.Send(new LogoutCommand(Guid.Parse(userId)), cancellationToken);
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
}
