using TNTTBD.Services.Domain.Enums;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Application.Contacts.DTOs;

public class ContactDetailDto : ContactDto
{
    public List<ContactYouthDto> Youths { get; set; } = new();
}

public class ContactYouthDto
{
    /// <summary>
    /// Link id (EucharisticYouthContact)
    /// </summary>
    public Guid Id { get; set; }
    public Guid EucharisticYouthId { get; set; }
    public string Code { get; set; } = string.Empty;
    public required FullName FullName { get; init; }
    public ContactRelationType ContactRelationType { get; set; }
}
