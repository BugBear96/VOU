using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using VOU.TenantCategories;

namespace VOU.MultiTenancy
{
    [Table("TenantWithSubCategory")]
    public class TenantWithSubCategory: Entity
    {
        public TenantWithSubCategory()
        {
            // EMPTY
        }

        public TenantWithSubCategory(Tenant tenant, TenantSubCategory subcategory)
        {
            Tenant = tenant;
            SubCategory = subcategory;
        }
        //public int TenantId { get; set; }
        public Tenant Tenant { get; set; }

        //public int SubCategoryId { get; set; }
        public TenantSubCategory SubCategory { get; set; }
    }
}
