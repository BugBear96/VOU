using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOU.Branch
{
    public interface ILocationManager
    {
        IQueryable<Location> Locations { get; }

        IQueryable<BranchWithVoucherPlatform> VoucherPlatforms { get; }

        Task CreateLocationAsync(Location location);

        Task<Location> FindLocationByNameAsync(string stateName);

        Task<Location> FindLocationAsync(int id);

        Task<BranchWithVoucherPlatform> CreateBranchWithVoucherPlatformAsync(BranchWithVoucherPlatform platform);

        Task<BranchWithVoucherPlatform> FindVoucherPlatformAsync(int id);

        Task UpdateBranchAsync(Location location);
    }
}
