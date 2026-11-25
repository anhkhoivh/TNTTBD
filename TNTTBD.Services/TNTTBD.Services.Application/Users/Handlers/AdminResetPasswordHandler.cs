using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Users.Commands;

namespace TNTTBD.Services.Application.Users.Handlers;

public class AdminResetPasswordHandler : IRequestHandler<AdminResetPasswordCommand>
{
    private readonly IUserRepository _userRepository;

    public AdminResetPasswordHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Handle(AdminResetPasswordCommand command, CancellationToken cancellationToken)
    {
        await _userRepository.ChangePasswordAsync(command.UserId, command.NewPassword, cancellationToken);
    }
}
