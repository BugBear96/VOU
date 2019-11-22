using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VOU.MultiTenancy;

namespace VOU.TenantCategories.Dto
{
    [AutoMapFrom(typeof(TenantCategory))]
    public class TenantCategoryEditDto : EntityDto, IShouldNormalize
    {
        [Required, StringLength(TenantCategory.MaxTitleLength)]
        public string Title { get; set; }

        public Boolean isActive { get; set; }

        public List<TenantSubCategory> SubCategories { get; set; }

        public void Normalize()
        {
            if (SubCategories == null)
                SubCategories = new List<TenantSubCategory>();
        }
    }
}
