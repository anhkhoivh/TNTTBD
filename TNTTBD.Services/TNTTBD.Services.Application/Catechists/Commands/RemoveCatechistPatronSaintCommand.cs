namespace TNTTBD.Services.Application.Catechists.Commands;

public record RemoveCatechistPatronSaintCommand(
    Guid CatechistId,
    string SaintName,
    int Day, 
    int Month
);