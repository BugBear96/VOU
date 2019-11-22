using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.MultiTenancy;
using VOU.TenantCategories;
using VOU.TenantCategories.Dto;

namespace VOU.MultiTenancy.Dto
{
    [AutoMapFrom(typeof(Tenant))]
    public class TenantDto : EntityDto
    {
        [Required]
        [StringLength(AbpTenantBase.MaxTenancyNameLength)]
        [RegularExpression(AbpTenantBase.TenancyNameRegex)]
        public string TenancyName { get; set; }

        [Required]
        [StringLength(AbpTenantBase.MaxNameLength)]
        public string Name { get; set; }      
        
        public TenantCategoryDto Category { get; set; }   
        
        public List<TenantSubCategoryDto> SubCategories { get; set; }

        public long? ProfilePictureId { get; set; }
        
        public bool IsActive {get; set;}
    }
}
