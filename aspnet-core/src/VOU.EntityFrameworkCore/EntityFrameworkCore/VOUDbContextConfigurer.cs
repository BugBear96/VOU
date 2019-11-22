using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace VOU.EntityFrameworkCore
{
    public static class VOUDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<VOUDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<VOUDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
