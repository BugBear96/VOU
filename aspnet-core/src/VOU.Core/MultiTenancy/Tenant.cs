using Abp.MultiTenancy;
using VOU.TenantCategories;
using VOU.Authorization.Users;
using System.Collections.Generic;

namespace VOU.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
        public virtual long? ProfilePictureId { get; protected set; }

        public TenantCategory Category { get; set; }

        public List<TenantWithSubCategory> SubCategories { get; set; }

        public Tenant()
        {
            //SubCategories = new List<TenantSubCategory>();
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
            SubCategories = new List<TenantWithSubCategory>();
        }

        public void UpdateCategory(TenantCategory category)
        {
            Category = category;
        }
        
        public void AddSubCategories(
            TenantWithSubCategory subCategory)
        {
            SubCategories.Add(subCategory);
        }

        public void ClearSubCategories()
        {
            SubCategories.Clear();
        }

        public void UpdateProfilePicture(long id)
        {
            ProfilePictureId = id;
        }
        
    }
}
