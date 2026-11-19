namespace TNTTBD.Services.Application.CatechismYears.Commands;

public record CreateCatechismYearCommand(
    DateOnly StartDate,
    DateOnly EndDate,
    string? Theme
);