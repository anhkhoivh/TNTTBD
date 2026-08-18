using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Application.Contacts.DTOs;

public class ContactDto
{
    public Guid Id { get; set; }
    public required FullName FullName { get; init; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
}
