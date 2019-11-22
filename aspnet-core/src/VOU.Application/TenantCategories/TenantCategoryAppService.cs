using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Extensions;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VOU.TenantCategories.Dto;

namespace VOU.TenantCategories
{
    public class TenantCategoryAppService : VOUAppServiceBase, ITenantCategoryAppService
    {
        private readonly ITenantCategoryManager _tenantCategoryManager;

        public TenantCategoryAppService(
            ITenantCategoryManager tenantCategoryManager)
        {
            _tenantCategoryManager = tenantCategoryManager;
        }

        public async Task<ListResultDto<TenantCategoryListDto>> GetTenantCategories(GetTenantCategoryInput input)
        {
            var items = await _tenantCategoryManager.TenantCategories
                .Where(x => (!input.Keyword.IsNullOrWhiteSpace()) ? x.Title.Contains(input.Keyword) : true)
                .Where(x => (input.IsActive.HasValue) ? x.isActive == input.IsActive : true)
                .Include(x => x.SubCategories)
                .Select(x => new TenantCategoryListDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    isActive = x.isActive,
                    SubCategories = x.SubCategories.Select(y => new TenantSubCategoryDto
                    {
                        Title = y.Title
                    }).ToList()
                })
                .OrderBy(x => x.Title)
                .ToListAsync();

            return new ListResultDto<TenantCategoryListDto>(items);

        }

        public async Task<TenantCategoryEditDto> GetTenantCategoryForEdit(EntityDto input)
        {
            var category = await _tenantCategoryManager.FindAsync(input.Id);
            if (category == null)
                throw new UserFriendlyException(L("InvalidCalendarPeriod"));

            return ObjectMapper.Map<TenantCategoryEditDto>(category);

        }

        public async Task<EntityDto> CreateOrUpdateTenantCategory(TenantCategoryEditDto input)
        {
            var isEdit = input.Id > 0;

            TenantCategory category;
            if (isEdit)
            {
                category = await _tenantCategoryManager.FindAsync(input.Id);
                if (category == null)
                    throw new UserFriendlyException(L("InvalidTenantCategory"));
            }
            else category = new TenantCategory(input.Title);

            category.UpdateName(input.Title);
            category.UpdateIsActive(input.isActive);

            category.ClearSubCategories();
            foreach (var item in input.SubCategories)
                category.AddSubCategories(item.Title);

            if (!isEdit)
                await _tenantCategoryManager.CreateAsync(category);

            await CurrentUnitOfWork.SaveChangesAsync();

            return new EntityDto(category.Id);
        }

        public async Task<HttpResponseMessage> UpdateIsActive(GetUpdateIsActiveInput input)
        {
            var category = await _tenantCategoryManager.FindAsync(input.CategoryId);
            if (category == null)
                throw new UserFriendlyException(L("InvalidTenantCategory"));

            category.UpdateIsActive(input.isActive);
            
            await CurrentUnitOfWork.SaveChangesAsync();

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            return await Task.FromResult(response);

        }
    }
}
