namespace TNTTBD.Services.Application.MilestoneEvents.Queries;

public record GenerateMilestoneCodesQuery(
    string? Template,
    int StartIndex,
    List<DateOnly> Dates
);
