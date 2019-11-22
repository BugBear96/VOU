using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using VOU.MultiTenancy.Dto;

namespace VOU.PublicClient.Dto
{
    public class TenantWithCategoryListDto : EntityDto
    {
        public string CategoryTitle { get; set; }

        public List<TenantDto> Tenants { get; set; }
    }
}
