using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using TNTTBD.Services.Infrastructure.Persistence.Catechism;
using System.IO;

namespace Nucleus.Services.MachineryOutputs.Infrastructure.DesignTime;

public class CatechismDbContextDesignTimeFactory : IDesignTimeDbContextFactory<CatechismDbContext>
{
    public CatechismDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var options = new DbContextOptionsBuilder<CatechismDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new CatechismDbContext(options);
    }
}
