using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MiniSkeletonAPI.Infrastructure.Data;

namespace MiniSkeletonAPI.Presentation.ContextFactory
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        //public ApplicationDbContext CreateDbContext(string[] args)
        //{
        //    var configuration = new ConfigurationBuilder()
        //    .SetBasePath(Directory.GetCurrentDirectory())
        //    .AddJsonFile("appsettings.json").Build();
        //    var builder = new DbContextOptionsBuilder<ApplicationDbContext>()
        //    .UseSqlite(configuration.GetConnectionString("DefaultConnection"),
        //    b => b.MigrationsAssembly("MiniSkeletonAPI.Presentation"));
        //    return new ApplicationDbContext(builder.Options);
        //}

        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("MiniSkeletonAPI.Presentation")
                );

            return new ApplicationDbContext(builder.Options);
        }
    }
}
