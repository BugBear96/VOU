using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using VOU.MultiTenancy;
using VOU.MultiTenancy.Dto;
using VOU.PublicClient.Dto;
using VOU.TenantCategories.Dto;
using VOU.Branch.Dto;
using VOU.Branch;
using VOU.TenantCategories;
using VOU.Voucher.Dto;
using VOU.Voucher;
using Abp.Authorization;
//using System.Linq.Dynamic.Core;

namespace VOU.PublicClient
{
    public class PublicClientAppService : VOUAppServiceBase, IPublicClientAppService
    {
        private readonly TenantManager _tenantManager;
        private readonly ITenantCategoryManager _tenantCategoryManager;
        private readonly ILocationManager _locationManager;
        private readonly IVoucherPlatformManager _voucherPlatformManager;
        public PublicClientAppService(
            TenantManager tenantManager,
            ITenantCategoryManager tenantCategoryManager,
            ILocationManager locationManager,
            IVoucherPlatformManager voucherPlatformManager
        )
        {
            _tenantManager = tenantManager;
            _tenantCategoryManager = tenantCategoryManager;
            _locationManager = locationManager;
            _voucherPlatformManager = voucherPlatformManager;

        }

        public async Task<ListResultDto<TenantWithCategoryListDto>> GetTenants()
        {
            
            var tenantCategories = await _tenantCategoryManager.TenantCategories.Include(x => x.SubCategories).ToListAsync();
            var tenants = await _tenantManager.Tenants
                            //.Where(x => (!input.Keyword.IsNullOrWhiteSpace()) ? x.TenancyName.Contains(input.Keyword) : true)
                            //.Where(x => (input.IsActive.HasValue) ? x.IsActive == input.IsActive : true)
                            .Where(x => x.Category != null)
                            .Include(x => x.Category)
                            //.Include(x => x.SubCategories)
                            .Include(x => x.SubCategories)
                            /*
                            .Select(x => new
                            {
                                x.Id,
                                CategoryId = x.Category.Id,
                                CategoryTitle = x.Category.Title,
                                x.SubCategories,
                                x.TenancyName,
                                x.IsActive,
                            })
                            */
                                
                            //.OrderBy(x => x.Key)
                            .ToListAsync();

            var items = tenants.GroupBy(x => x.Category.Title)
                .Select(x =>
                {
                    var t = x.Select(y => {
                        //var categoryDto = new TenantCategoryDto();
                        var subCategoriesDto = new List<TenantSubCategoryDto>();
                        //categoryDto.Title = y.Category.Title;
                        var subCategories = tenantCategories.Where(z => z.Id == y.Category.Id)
                            .Select(z => new {
                                subCategories = z.SubCategories.ToDictionary(z1 => z1.Id, z1 => z1.Title)
                            }).FirstOrDefault().subCategories;

                        foreach (var subCategory in y.SubCategories)
                        {
                            var s = new TenantSubCategoryDto();
                            s.Title = subCategories.GetOrDefault(subCategory.SubCategory.Id);
                            subCategoriesDto.Add(s);
                        }
                            

                        return new TenantDto
                        {
                            Id = y.Id,
                            TenancyName = y.TenancyName,
                            //Name = y.Name,
                            IsActive = y.IsActive,
                            ProfilePictureId = y.ProfilePictureId,
                            //Category = categoryDto,
                            //new TenantCategory.Dto.TenantCategoryDto
                            //{
                            //    Title = x.Category.Title
                            //},
                            //x.SubCategories.FToList()
                            SubCategories = subCategoriesDto
                            //x.SubCategories.Select(y => new TenantCategory.Dto.TenantSubCategoryDto
                            //{
                            //    Title = y.Title
                            //}).ToList()
                        };
                    }).ToList();

                    return new TenantWithCategoryListDto
                    {
                        CategoryTitle = x.Key,
                        Tenants = t
                    };
                })
                .ToList();
            return new ListResultDto<TenantWithCategoryListDto>(items);
            
            /*
            var items = tenants
                .Select(x =>
                {
                    var categoryDto = new TenantCategoryDto();
                    var subCategoriesDto = new List<TenantSubCategoryDto>();
                    if (x.Category != null)
                    {
                        categoryDto.Title = x.Category.Title;
                        var subCategories = _tenantCategoryManager.TenantCategories
                        .Where(y => y.Id == x.Category.Id).Include(y => y.SubCategories)
                        .Select(y => new {
                            subCategories = y.SubCategories.ToDictionary(z => z.Id, z => z.Title)
                        }).FirstOrDefault().subCategories;

                        foreach (var subCategory in x.SubCategories)
                        {
                            var s = new TenantSubCategoryDto();
                            s.Title = subCategories.GetOrDefault(subCategory.SubCategory.Id);
                            subCategoriesDto.Add(s);
                        }
                    }

                    return new TenantDto
                    {
                        Id = x.Id,
                        TenancyName = x.TenancyName,
                        Name = x.Name,
                        IsActive = x.IsActive,
                        Category = categoryDto,
                        //new TenantCategory.Dto.TenantCategoryDto
                        //{
                        //    Title = x.Category.Title
                        //},
                        //x.SubCategories.FToList()
                        SubCategories = subCategoriesDto
                        //x.SubCategories.Select(y => new TenantCategory.Dto.TenantSubCategoryDto
                        //{
                        //    Title = y.Title
                        //}).ToList()
                    };
                })
                .ToList();

            var category = await _tenantManager.Tenants
                .Where(x => x.IsActive == true)
                .Where(x => x.Category != null)
                .Include(x => x.Category)
                .GroupBy(x => x.Category.Title)
                .OrderBy(x => x.Key)
                .ToList();
                */

            return new ListResultDto<TenantWithCategoryListDto>();

        }

        public async Task<TenantBranchesListDto> GetLocations(EntityDto input)
        {


            var items = await _locationManager.Locations
                    .Where(x => x.TenantId == input.Id)
                   //.Where(x => (!input.Keyword.IsNullOrWhiteSpace()) ? x.Name.Contains(input.Keyword) : true)
                   .Select(x => new LocationListDto
                   {
                       Id = x.Id,
                       Name = x.Name,
                       Address = x.Address,
                       Postcode = x.Postcode,
                       State = new StateDto
                       {
                           Id = x.State.Id,
                           StateName = x.State.StateName
                       },
                       City = new CityDto
                       {
                           CityName = x.City.CityName
                       },
                       CoverPictureId = x.CoverPictureId ?? 0,
                       Remarks = x.Remarks
                   })
                   .OrderBy(x => x.Name)
                   .ToListAsync();

            var tenant = await _tenantManager.Tenants
                //.Where(x => (!input.Keyword.IsNullOrWhiteSpace()) ? x.TenancyName.Contains(input.Keyword) : true)
                .Where(x => x.Id == input.Id)
                .Include(x => x.Category)
                .Include(x => x.SubCategories)
                //.Include(x => x.SubCategories)
                .OrderBy(x => x.TenancyName)
                .ToListAsync();

            var output = tenant
                .Select(x =>
                {
                    var categoryDto = new TenantCategoryDto();
                    var subCategoriesDto = new List<TenantSubCategoryDto>();
                    if (x.Category != null)
                    {
                        categoryDto.Title = x.Category.Title;
                        var subCategories = _tenantCategoryManager.TenantCategories
                        .Where(y => y.Id == x.Category.Id).Include(y => y.SubCategories)
                        .Select(y => new {
                            subCategories = y.SubCategories.ToDictionary(z => z.Id, z => z.Title)
                        }).FirstOrDefault().subCategories;

                        foreach (var subCategory in x.SubCategories)
                        {
                            var s = new TenantSubCategoryDto();
                            s.Title = subCategories.GetOrDefault(subCategory.SubCategory.Id);
                            subCategoriesDto.Add(s);
                        }
                    }

                    return new TenantBranchesListDto
                    {
                        Id = x.Id,
                        TenancyName = x.TenancyName,
                        Name = x.Name,
                        IsActive = x.IsActive,
                        Category = categoryDto,
                        SubCategories = subCategoriesDto,
                        Locations = items
                        
                    };
                })
                .ToList().FirstOrDefault();

            return output;

        }

        public async Task<ListResultDto<VoucherPlatformListDto>> GetVoucherPlatforms(EntityDto input)
        {
            var items = await _voucherPlatformManager.VoucherPlatforms
                //.Where(x => (!input.Keyword.IsNullOrWhiteSpace()) ? x.Name.Contains(input.Keyword) : true)
                //.WhereIf(input.ShowArchived, x => x.ArchivedTime != null)
                //.WhereIf(!input.ShowArchived, x => x.ArchivedTime == null)
                .Where(x => x.ArchivedTime == null)
                .Select(x => new VoucherPlatformListDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    ArchivedTime = x.ArchivedTime,
                    CoverPictureId = x.CoverPictureId ?? 0
                })
                .OrderBy(x => x.Name)
                .ToListAsync();
            return new ListResultDto<VoucherPlatformListDto>(items);
           
        }

        
    }
}
