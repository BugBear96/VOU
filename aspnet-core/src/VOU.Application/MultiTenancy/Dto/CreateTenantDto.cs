using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.MultiTenancy;
using Abp.Runtime.Validation;
using VOU.TenantCategories;

namespace VOU.MultiTenancy.Dto
{
    [AutoMapTo(typeof(Tenant))]
    public class CreateTenantDto : IShouldNormalize
    {
        [Required]
        [StringLength(AbpTenantBase.MaxTenancyNameLength)]
        [RegularExpression(AbpTenantBase.TenancyNameRegex)]
        public string TenancyName { get; set; }

        [Required]
        [StringLength(AbpTenantBase.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string AdminEmailAddress { get; set; }

        [StringLength(AbpTenantBase.MaxConnectionStringLength)]
        public string ConnectionString { get; set; }
        public TenantCategory Category { get; set; }
        public List<TenantWithSubCategory> SubCategories { get; set; }

        public List<TenantSubCategory> _subCategories { get; set; }

        public bool IsActive {get; set;}

        public void Normalize()
        {
            if (SubCategories == null)
                SubCategories = new List<TenantWithSubCategory>();
        }
    }
}
