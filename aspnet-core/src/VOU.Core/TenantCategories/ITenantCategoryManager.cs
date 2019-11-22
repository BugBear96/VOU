using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VOU.TenantCategories
{
    public interface ITenantCategoryManager
    {
        IQueryable<TenantCategory> TenantCategories { get; }

        Task CreateAsync(TenantCategory category);

        Task<TenantCategory> FindByNameAsync(string name);

        Task<TenantCategory> FindAsync(int id);
    }
}
