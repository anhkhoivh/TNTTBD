namespace TNTTBD.Services.Application.Dashboard.DTOs;

public class DashboardSummaryDto
{
    public string CurrentYearTitle { get; set; } = "—";
    public string? CurrentYearTheme { get; set; }
    public List<DashboardClassDto> MyClasses { get; set; } = [];
    public List<DashboardSessionDto> UpcomingSessions { get; set; } = [];
    public DashboardTotalsDto? Totals { get; set; }
}

public class DashboardClassDto
{
    public Guid Id { get; set; }
    public required string ClassName { get; set; }
}

public class DashboardSessionDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public DateTimeOffset DateTime { get; set; }
    public Guid CatechismClassId { get; set; }
    public required string ClassName { get; set; }
}

public class DashboardTotalsDto
{
    public int Youth { get; set; }
    public int Catechists { get; set; }
    public int Classes { get; set; }
    public int Years { get; set; }
}
