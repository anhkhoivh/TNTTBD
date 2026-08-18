using MediatR;
using TNTTBD.Services.Application.Auth.Commands;
using TNTTBD.Services.Application.Auth.Interfaces;

namespace TNTTBD.Services.Application.Auth.Handlers;

public class ChangeOwnPasswordHandler : IRequestHandler<ChangeOwnPasswordCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthorizationContext _authContext;

    public ChangeOwnPasswordHandler(IUserRepository userRepository, IAuthorizationContext authContext)
    {
        _userRepository = userRepository;
        _authContext = authContext;
    }

    public async Task Handle(ChangeOwnPasswordCommand command, CancellationToken cancellationToken)
    {
        var userId = _authContext.GetCurrentUserId()
            ?? throw new UnauthorizedAccessException("Not authenticated.");

        var user = await _userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new UnauthorizedAccessException("User not found.");

        var valid = await _userRepository.ValidatePasswordAsync(user.Email, command.CurrentPassword, cancellationToken);
        if (!valid)
            throw new UnauthorizedAccessException("Current password is incorrect.");

        await _userRepository.ChangePasswordAsync(userId, command.NewPassword, cancellationToken);
    }
}
