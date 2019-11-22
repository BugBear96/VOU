using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOU.Voucher
{
    public class VoucherPlatformManager : VOUDomainServiceBase, IVoucherPlatformManager
    {

        private readonly IRepository<VoucherPlatform> _repository;

        public VoucherPlatformManager(
            IRepository<VoucherPlatform> repository)
        {
            _repository = repository;
        }

        public IQueryable<VoucherPlatform> VoucherPlatforms => _repository.GetAll();

        public Task CreateAsync(VoucherPlatform voucherPlatform)
        {
            return _repository.InsertAsync(voucherPlatform);
        }

        public Task UpdateVoucherPlatformAsync(VoucherPlatform voucherPlatform)
        {
            return _repository.UpdateAsync(voucherPlatform);
        }

        public Task<VoucherPlatform> FindAsync(int id, bool considerArchived)
        {
            try
            {
                return _repository.GetAll()
                .Where(x => x.Id == id)
                .Where(x => (!considerArchived) ? x.ArchivedTime == null : true)
                //.WhereIf(!considerArchived, x => x.ArchivedTime == null)
                .FirstOrDefaultAsync();
            }
            catch(Exception e)
            {
                return _repository.GetAll().Where(x => x.Id == id).FirstOrDefaultAsync();
            }
            
        }

        public Task<VoucherPlatform> FindByNameAsync(string name)
        {
            return _repository.GetAll()
                .Where(x => x.Name == name && x.ArchivedTime == null)
                .FirstOrDefaultAsync();
        }

    }
}
