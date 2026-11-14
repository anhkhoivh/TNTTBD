using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Application.MilestoneEvents.DTOs;

/// <summary>
/// Godparent contact
/// </summary>
public class MilestoneSponsorDto
{
    public Guid ContactId { get; set; }
    public FullName FullName { get; set; } = null!;
    public string? PhoneNumber { get; set; }
}
