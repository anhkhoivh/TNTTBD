namespace TNTTBD.Services.Application.CatechismClasses.Commands;

public record CreateCatechismClassCommand(
    Guid CatechismYearId,
    string Name,
    string Code
);