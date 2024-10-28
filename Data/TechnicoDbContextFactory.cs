namespace Technico.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

public class TechnicoDbContextFactory : IDesignTimeDbContextFactory<TechnicoDbContext>
{
    public TechnicoDbContext CreateDbContext(string[] args)
    {
        // Build configuration
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        // Set up DbContext options
        var optionsBuilder = new DbContextOptionsBuilder<TechnicoDbContext>();
        var connectionString = configuration.GetConnectionString("TechnicoDb");

        optionsBuilder.UseSqlServer(connectionString);

        return new TechnicoDbContext(optionsBuilder.Options);
    }
}
