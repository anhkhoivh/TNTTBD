namespace TNTTBD.Services.Application.Contacts.Commands;

/// <summary>
/// Unlink a contact from a youth. <paramref name="Id"/> is the link id.
/// </summary>
public record UnlinkContactFromYouthCommand(Guid Id);
