using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Application.Catechists.DTOs;

public class CatechistDto
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Full Name
    /// </summary>
    public required FullName FullName { get; init; }

    /// <summary>
    /// Date Of Birth
    /// </summary>
    public DateOnly? DateOfBirth { get; set; }

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