namespace TNTTBD.Services.Application.Catechists.DTOs;

public class CatechistCurrentClassDto
{
    public Guid ClassId { get; set; }
    public required string ClassName { get; set; }
    public Guid CatechismYearId { get; set; }
    public required string CatechismYearTitle { get; set; }
}
