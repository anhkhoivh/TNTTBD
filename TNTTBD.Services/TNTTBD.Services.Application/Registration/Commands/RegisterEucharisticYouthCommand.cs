using MediatR;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.Registration.Commands;

public record RegisterEucharisticYouthCommand(
    YouthData Youth,
    List<ContactData> Contacts,
    string? AccountEmail
) : IRequest<Guid>;

public record YouthData(
    FullNameData FullName,
    DateOnly DateOfBirth,
    bool IsMale,
    string? Address,
    BaptismData? Baptism
);

public record FullNameData(
    string GivenName,
    string? FamilyName,
    string? MiddleName,
    string? ChristName
);

public record ContactData(
    string GivenName,
    string? FamilyName,
    ContactRelationType Relation,
    string PhoneNumber,
    string? Email
);

public record BaptismData(
    DateOnly Date,
    string? Place
);
