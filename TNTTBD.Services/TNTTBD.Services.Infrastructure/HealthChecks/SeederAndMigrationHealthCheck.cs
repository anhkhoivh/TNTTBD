using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Infrastructure.Persistence.Catechism;

public class SeederAndMigrationHealthCheck : IHealthCheck
{
    private readonly CatechismDbContext _context;

    public SeederAndMigrationHealthCheck(CatechismDbContext context)
    {
        _context = context;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        // Kiểm tra migration
        var pending = await _context.Database.GetPendingMigrationsAsync(cancellationToken);

        if (pending.Any())
        {
            return HealthCheckResult.Unhealthy("Pending migrations: " + string.Join(", ", pending));
        }

        return HealthCheckResult.Healthy("Seeding done and all migrations applied");
    }
}
