namespace TNTTBD.Services.Application.Common.DTOs;

/// <summary>
/// Patron saint Dto
/// </summary>
public class PatronSaintDto
{
    public required string SaintName { get; set; }
    public int Day { get; set; }
    public int Month { get; set; }
}