using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace VOU.TenantCategories.Dto
{
    public class TenantCategoryListDto : EntityDto
    {
        public string Title { get; set; }

        public Boolean isActive { get; set; }
        public List<TenantSubCategoryDto> SubCategories { get; set; }

    }
}
