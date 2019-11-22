using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VOU.TenantCategories.Dto
{
    [AutoMapFrom(typeof(TenantCategory))]
    public class TenantCategoryDto
    {
        [Required, StringLength(TenantCategory.MaxTitleLength)]
        public string Title { get; set; }
    }
}
