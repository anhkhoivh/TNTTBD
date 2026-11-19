using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.DTOs;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.Users.Commands;
using TNTTBD.Services.Application.Users.Queries;

namespace TNTTBD.Services.Api.Controllers;

[ApiController]
[Route("api/users")]
[Authorize(Roles = "Admin")]
public class UsersController : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetUsersQuery query,
        [FromServices] IRequestHandler<GetUsersQuery, PaginatedResult<UserDto>> handler,
        CancellationToken cancellationToken)
    {
        PaginatedResult<UserDto> result = await handler.Handle(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("detail")]
    public async Task<IActionResult> GetById(
        [FromQuery] GetUserByIdQuery query,
        [FromServices] IRequestHandler<GetUserByIdQuery, UserDto?> handler,
        CancellationToken cancellationToken)
    {
        UserDto? user = await handler.Handle(query, cancellationToken);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateUserCommand command,
        [FromServices] IRequestHandler<CreateUserCommand, Guid> handler,
        CancellationToken cancellationToken)
    {
        Guid id = await handler.Handle(command, cancellationToken);
        return Created($"/api/users?id={id}", id);
    }

    [HttpPatch]
    public async Task<IActionResult> Update(
        [FromBody] UpdateUserCommand command,
        [FromServices] IRequestHandler<UpdateUserCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.Handle(command, cancellationToken);
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(
        [FromBody] RemoveUserCommand command,
        [FromServices] IRequestHandler<RemoveUserCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.Handle(command, cancellationToken);
        return NoContent();
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(
        [FromBody] AdminResetPasswordCommand command,
        [FromServices] IRequestHandler<AdminResetPasswordCommand> handler,
        CancellationToken cancellationToken)
    {
        await handler.Handle(command, cancellationToken);
        return Ok();
    }
}
