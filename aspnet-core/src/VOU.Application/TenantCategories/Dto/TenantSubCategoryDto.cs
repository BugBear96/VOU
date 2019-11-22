using Abp.AutoMapper;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VOU.TenantCategories.Dto
{
    [AutoMapFrom(typeof(TenantSubCategory))]
    public class TenantSubCategoryDto
    {
        [Required, StringLength(TenantSubCategory.MaxTitleLength)]
        public string Title { get; set; }

    }
}
