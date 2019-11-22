using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOU.Branch
{
    public class LocationManager : VOUDomainServiceBase, ILocationManager
    {
        private readonly IRepository<Location> _locationRepository;
        private readonly IRepository<BranchWithVoucherPlatform> _voucherPlatformRepository;

        public LocationManager(
            IRepository<Location> locationRepository,
            IRepository<BranchWithVoucherPlatform> voucherPlatformRepository)
        {
            _locationRepository = locationRepository;
            _voucherPlatformRepository = voucherPlatformRepository;
        }

        public IQueryable<Location> Locations => _locationRepository.GetAll();

        public IQueryable<BranchWithVoucherPlatform> VoucherPlatforms => _voucherPlatformRepository.GetAll();

        public Task CreateLocationAsync(Location location)
        {
            return _locationRepository.InsertAsync(location);
        }

        public Task<Location> FindLocationAsync(int id)
        {
            return _locationRepository.GetAll()
                .Where(x => x.Id == id)
                .Include(x => x.State)
                .Include(x => x.City)
                .FirstOrDefaultAsync();
        }

        public Task<Location> FindLocationByNameAsync(string name)
        {
            return _locationRepository.GetAll()
                .Where(x => x.Name == name)
                .Include(x => x.State)
                .Include(x => x.City)
                .FirstOrDefaultAsync();
        }

        public Task<BranchWithVoucherPlatform> CreateBranchWithVoucherPlatformAsync(BranchWithVoucherPlatform platform)
        {
            return _voucherPlatformRepository.InsertAsync(platform);
        }

        public Task<BranchWithVoucherPlatform> FindVoucherPlatformAsync(int id)
        {
            return _voucherPlatformRepository.GetAll()
                .Where(x => x.Id == id)
                .Include(x => x.Location)
                .Include(x => x.VoucherPlatform)
                .FirstOrDefaultAsync();
        }

        public Task UpdateBranchAsync(Location location)
        {
            return _locationRepository.UpdateAsync(location);
        }
    }
}
