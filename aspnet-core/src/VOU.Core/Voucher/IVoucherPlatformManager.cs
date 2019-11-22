using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOU.Voucher
{
    public interface IVoucherPlatformManager 
    {

        IQueryable<VoucherPlatform> VoucherPlatforms { get; }

        Task CreateAsync(VoucherPlatform pricingScheme);

        Task<VoucherPlatform> FindByNameAsync(string name);

        Task<VoucherPlatform> FindAsync(int id, bool considerArchived = false);

        Task UpdateVoucherPlatformAsync(VoucherPlatform voucherPlatform);
    }
}
