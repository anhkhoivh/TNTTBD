using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Users.Commands;

namespace TNTTBD.Services.Application.Users.Handlers;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;

    public CreateUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Guid> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        return await _userRepository.CreateAsync(
            command.Email,
            command.UserName,
            command.Password,
            command.Role,
            command.CatechistId,
            command.EucharisticYouthId,
            command.ContactId,
            cancellationToken);
    }
}
