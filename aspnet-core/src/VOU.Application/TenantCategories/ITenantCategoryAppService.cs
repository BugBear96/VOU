using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VOU.TenantCategories.Dto;

namespace VOU.TenantCategories
{
    public interface ITenantCategoryAppService : IApplicationService
    {
        Task<ListResultDto<TenantCategoryListDto>> GetTenantCategories(GetTenantCategoryInput input);

        Task<TenantCategoryEditDto> GetTenantCategoryForEdit(EntityDto input);

        Task<EntityDto> CreateOrUpdateTenantCategory(TenantCategoryEditDto input);

        Task<HttpResponseMessage> UpdateIsActive(GetUpdateIsActiveInput input);
    }
}
