namespace TNTTBD.Services.Application.CatechismClasses.Commands;

public record UpdateCatechismClassCommand(
    Guid CatechismClassId,
    string ClassName,
    string ClassCode
);