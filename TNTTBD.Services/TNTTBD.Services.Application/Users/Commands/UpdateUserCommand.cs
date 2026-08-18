using MediatR;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.Users.Commands;

public record UpdateUserCommand(
    Guid Id,
    string? UserName,
    bool? IsActive,
    UserRole? Role,
    UserEntityLink? Link = null
) : IRequest;

/// Optional link update. Kind=None clears the link; Kind=Catechist|Youth|Contact sets the corresponding FK and nulls the others.
public record UserEntityLink(UserEntityLinkKind Kind, Guid? Id);

public enum UserEntityLinkKind
{
    None = 0,
    Catechist = 1,
    Youth = 2,
    Contact = 3,
}
