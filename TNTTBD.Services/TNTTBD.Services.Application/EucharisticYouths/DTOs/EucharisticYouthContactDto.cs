using TNTTBD.Services.Domain.Enums;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Application.EucharisticYouths.DTOs;

public class EucharisticYouthContactDto
{
    public Guid Id { get; set; }
    public Guid EucharisticYouthId { get; set; }
    public Guid ContactId { get; set; }
    public ContactRelationType ContactRelationType { get; set; }
    public ContactInfoDto Contact { get; set; } = null!;
}

public class ContactInfoDto
{
    public Guid Id { get; set; }
    /// <summary>
    /// Full Name
    /// </summary>
    public required FullName FullName { get; init; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
}
