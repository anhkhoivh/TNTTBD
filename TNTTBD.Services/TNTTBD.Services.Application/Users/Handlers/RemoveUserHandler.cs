using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Users.Commands;

namespace TNTTBD.Services.Application.Users.Handlers;

public class RemoveUserHandler : IRequestHandler<RemoveUserCommand>
{
    private readonly IUserRepository _userRepository;

    public RemoveUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Handle(RemoveUserCommand command, CancellationToken cancellationToken)
    {
        await _userRepository.DeactivateAsync(command.Id, cancellationToken);
    }
}
