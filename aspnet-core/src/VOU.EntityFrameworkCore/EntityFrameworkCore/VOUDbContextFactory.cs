using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using VOU.Configuration;
using VOU.Web;

namespace VOU.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class VOUDbContextFactory : IDesignTimeDbContextFactory<VOUDbContext>
    {
        public VOUDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<VOUDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            VOUDbContextConfigurer.Configure(builder, configuration.GetConnectionString(VOUConsts.ConnectionStringName));

            return new VOUDbContext(builder.Options);
        }
    }
}
