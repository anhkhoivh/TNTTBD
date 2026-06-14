using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Application.Catechists.DTOs;

public class CatechistDto
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Christ Name
    /// </summary>
    public string? ChristName { get; set; }

    /// <summary>
    /// Family Name
    /// </summary>
    public string? FamilyName { get; set; }

    /// <summary>
    /// Middle Name
    /// </summary>
    public string? MiddleName { get; set; }

    /// <summary>
    /// Given Name
    /// </summary>
    public required string GivenName { get; set; }

    /// <summary>
    /// Date Of Birth
    /// </summary>
    public DateOnly DateOfBirth { get; set; }

    /// <summary>
    /// Address
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Phone Number
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Is male
    /// </summary>
    public bool IsMale { get; set; }

    public List<PhotoReferenceDto> Photos { get; set; } = new List<PhotoReferenceDto>();
    public List<PatronSaintDto> PatronSaints { get; set; } = new List<PatronSaintDto>();
}