using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using VOU.Authorization.Roles;
using VOU.Authorization.Users;
using VOU.MultiTenancy;
using VOU.BinaryObjects;
using VOU.TenantCategories;

namespace VOU.EntityFrameworkCore
{
    public class VOUDbContext : AbpZeroDbContext<Tenant, Role, User, VOUDbContext>
    {
        /* Define a DbSet for each entity of the application */

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public DbSet<TenantWithSubCategory> TenantWithSubCategories { get; set; }

        public DbSet<Branch.BranchWithVoucherPlatform> BranchWithVoucherPlatforms { get; set; }
        public DbSet<Branch.City> Cities { get; set; }
        public DbSet<Branch.State> States { get; set; }
        public DbSet<Branch.Location> Locations { get; set; }

        public DbSet<Voucher.VoucherPlatform> VoucherPlatforms { get; set; }

        public DbSet<TenantCategory> TenantCategories { get; set; }
        public DbSet<TenantSubCategory> TenantSubCategories { get; set; }

        public VOUDbContext(DbContextOptions<VOUDbContext> options)
            : base(options)
        {
        }
    }
}
