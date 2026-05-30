using MediatR;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Users.Commands;

namespace TNTTBD.Services.Application.Users.Handlers;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        await _userRepository.UpdateAsync(command.Id, command.UserName, command.IsActive, command.Role, cancellationToken);
    }
}
