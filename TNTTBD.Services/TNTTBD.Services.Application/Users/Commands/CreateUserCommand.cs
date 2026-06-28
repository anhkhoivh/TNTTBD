using MediatR;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.Users.Commands;

public record CreateUserCommand(
    string Email,
    string UserName,
    string Password,
    UserRole Role
) : IRequest<Guid>;
