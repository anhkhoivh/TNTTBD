using MediatR;

namespace TNTTBD.Services.Application.Contacts.Commands;

public record UpdateContactCommand(
    Guid Id,
    string GivenName,
    string? ChristName,
    string? FamilyName,
    string? MiddleName,
    string? PhoneNumber,
    string? Email
) : IRequest<bool>;
