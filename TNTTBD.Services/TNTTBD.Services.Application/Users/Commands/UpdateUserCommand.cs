using MediatR;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.Users.Commands;

public record UpdateUserCommand(
    Guid Id,
    string? UserName,
    bool? IsActive,
    UserRole? Role
) : IRequest;
