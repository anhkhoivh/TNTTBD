using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism;

public static class CatechismDbContextExtensions
{
    private static readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(30);

    /// <summary> Registers a scoped instance of <see cref="CatechismDbContext"/> </summary>
    public static IServiceCollection AddCatechismDbContext(
        this IServiceCollection services,
        string? connectionString,
        TimeSpan? commandTimeout = null,
        Action<DbContextOptionsBuilder>? configureOptions = null)
    {
        services.AddDbContext<CatechismDbContext>((serviceProvider, options) =>
        {
            options
                .AddInterceptors([.. serviceProvider.GetServices<ISaveChangesInterceptor>()])
                .UseNpgsql(connectionString, npgsqlOptions =>
                {
                    npgsqlOptions.CommandTimeout((int)(commandTimeout ?? _defaultTimeout).TotalSeconds);
                });

            configureOptions?.Invoke(options);
        });

        return services;
    }
}
