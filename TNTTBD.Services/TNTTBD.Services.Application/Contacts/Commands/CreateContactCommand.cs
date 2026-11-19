using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.Contacts.Commands;

public record CreateContactCommand(
    string GivenName,
    string? ChristName,
    string? FamilyName,
    string? MiddleName,
    string? PhoneNumber,
    string? Email,
    Guid? LinkEucharisticYouthId = null,
    ContactRelationType? LinkRelationType = null
);
