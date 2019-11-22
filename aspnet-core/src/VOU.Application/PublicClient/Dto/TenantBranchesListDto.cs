using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using VOU.Branch.Dto;
using VOU.MultiTenancy;
using VOU.TenantCategories.Dto;

namespace VOU.PublicClient.Dto
{
    [AutoMapFrom(typeof(Tenant))]
    public class TenantBranchesListDto : EntityDto
    {
       
        public string TenancyName { get; set; }

        public string Name { get; set; }

        public TenantCategoryDto Category { get; set; }

        public List<TenantSubCategoryDto> SubCategories { get; set; }

        public bool IsActive { get; set; }

        public List<LocationListDto> Locations { get; set; }
    }
}
