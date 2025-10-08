using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Video_Media_Processor.Backend.Data
{
    public class ApiDataContextFactory : IDesignTimeDbContextFactory<ApiDataContext>
    {
        public ApiDataContext CreateDbContext(string[] args)
        {
            // Build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Build DbContextOptions
            var optionsBuilder = new DbContextOptionsBuilder<ApiDataContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseNpgsql(connectionString);

            return new ApiDataContext(optionsBuilder.Options);
        }
    }
}
