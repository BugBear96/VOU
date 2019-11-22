using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOU.TenantCategories
{
    public class TenantCategoryManager : VOUDomainServiceBase, ITenantCategoryManager
    {
        private readonly IRepository<TenantCategory> _repository;

        public TenantCategoryManager(
            IRepository<TenantCategory> repository)
        {
            _repository = repository;
        }

        public IQueryable<TenantCategory> TenantCategories => _repository.GetAll();

        public Task CreateAsync(TenantCategory tenantCategory)
        {
            return _repository.InsertAsync(tenantCategory);
        }

        public Task<TenantCategory> FindAsync(int id)
        {
            return _repository.GetAll()
                .Where(x => x.Id == id)
                .Include(x => x.SubCategories)
                .FirstOrDefaultAsync();
        }

        public Task<TenantCategory> FindByNameAsync(string title)
        {
            return _repository.GetAll()
                .Where(x => x.Title == title)
                .Include(x => x.SubCategories)
                .FirstOrDefaultAsync();
        }
    }
}
