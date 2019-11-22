using Abp.Application.Features;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using System.Linq;
using VOU.Authorization.Users;
using VOU.Editions;

namespace VOU.MultiTenancy
{
    public class TenantManager : AbpTenantManager<Tenant, User>
    {
        //private readonly IRepository<Tenant> _repository;

        //public IQueryable<Tenant> AllTenants => _repository.GetAll();

        public TenantManager(
            IRepository<Tenant> tenantRepository, 
            IRepository<TenantFeatureSetting, long> tenantFeatureRepository, 
            EditionManager editionManager,
            IAbpZeroFeatureValueStore featureValueStore) 
            : base(
                tenantRepository, 
                tenantFeatureRepository, 
                editionManager,
                featureValueStore)
        {
        }
    }
}
