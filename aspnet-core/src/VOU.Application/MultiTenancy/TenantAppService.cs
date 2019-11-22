using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.MultiTenancy;
using Abp.Runtime.Security;
using VOU.Authorization;
using VOU.Authorization.Roles;
using VOU.Authorization.Users;
using VOU.Editions;
using VOU.MultiTenancy.Dto;
using Microsoft.AspNetCore.Identity;
using VOU.TenantCategories;
using System.Collections.Generic;
using System;
using VOU.TenantCategories.Dto;
using Abp.Collections.Extensions;

namespace VOU.MultiTenancy
{
    [AbpAuthorize(PermissionNames.Pages_Tenants)]
    public class TenantAppService : AsyncCrudAppService<Tenant, TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>, ITenantAppService
    {
        private readonly TenantManager _tenantManager;
        private readonly EditionManager _editionManager;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IAbpZeroDbMigrator _abpZeroDbMigrator;

        private readonly ITenantCategoryManager _tenantCategoryManager;

        public TenantAppService(
            IRepository<Tenant, int> repository,
            TenantManager tenantManager,
            EditionManager editionManager,
            UserManager userManager,
            RoleManager roleManager,
            IAbpZeroDbMigrator abpZeroDbMigrator,

            ITenantCategoryManager tenantCategoryManager
            )
            : base(repository)
        {
            _tenantManager = tenantManager;
            _editionManager = editionManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _abpZeroDbMigrator = abpZeroDbMigrator;

            _tenantCategoryManager = tenantCategoryManager;
        }

        public override async Task<TenantDto> Create(CreateTenantDto input)
        {
            CheckCreatePermission();

            var category = await _tenantCategoryManager.TenantCategories
                .Where(x => x.Id == input.Category.Id)
                .Include(x => x.SubCategories)
                .OrderBy(x => x.Title).ToListAsync();

            input.Category = category.FirstOrDefault();
            // Create tenant
            var tenant = ObjectMapper.Map<Tenant>(input);
            tenant.ConnectionString = input.ConnectionString.IsNullOrEmpty()
                ? null
                : SimpleStringCipher.Instance.Encrypt(input.ConnectionString);

            var defaultEdition = await _editionManager.FindByNameAsync(EditionManager.DefaultEditionName);
            if (defaultEdition != null)
            {
                tenant.EditionId = defaultEdition.Id;
            }

            await _tenantManager.CreateAsync(tenant);
            await CurrentUnitOfWork.SaveChangesAsync(); // To get new tenant's id.

            foreach (var subcategory in category.FirstOrDefault().SubCategories.ToList())
            {
                if (input._subCategories.Exists(x => x.Title == subcategory.Title))
                {
                    var twsc = new TenantWithSubCategory(tenant, subcategory);
                    await CurrentUnitOfWork.SaveChangesAsync();

                    tenant.AddSubCategories(twsc);
                }
            }

            await CurrentUnitOfWork.SaveChangesAsync(); // To get new tenant's id.
                                                        // Create tenant database
            _abpZeroDbMigrator.CreateOrMigrateForTenant(tenant);

            // We are working entities of new tenant, so changing tenant filter
            using (CurrentUnitOfWork.SetTenantId(tenant.Id))
            {
                // Create static roles for new tenant
                CheckErrors(await _roleManager.CreateStaticRoles(tenant.Id));

                await CurrentUnitOfWork.SaveChangesAsync(); // To get static role ids

                // Grant all permissions to admin role
                var adminRole = _roleManager.Roles.Single(r => r.Name == StaticRoleNames.Tenants.Admin);
                await _roleManager.GrantAllPermissionsAsync(adminRole);

                // Create admin user for the tenant
                var adminUser = User.CreateTenantAdminUser(tenant.Id, input.AdminEmailAddress);
                await _userManager.InitializeOptionsAsync(tenant.Id);
                CheckErrors(await _userManager.CreateAsync(adminUser, User.DefaultPassword));
                await CurrentUnitOfWork.SaveChangesAsync(); // To get admin user's id

                // Assign admin user to role!
                CheckErrors(await _userManager.AddToRoleAsync(adminUser, adminRole.Name));
                await CurrentUnitOfWork.SaveChangesAsync();
            }

            var tenantDto = await GetTenant(new EntityDto<long>(tenant.Id));
            return tenantDto;
        }

        protected override IQueryable<Tenant> CreateFilteredQuery(PagedTenantResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.TenancyName.Contains(input.Keyword) || x.Name.Contains(input.Keyword))
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive);
        }

        protected override void MapToEntity(TenantDto updateInput, Tenant entity)
        {
            // Manually mapped since TenantDto contains non-editable properties too.
            entity.Name = updateInput.Name;
            entity.TenancyName = updateInput.TenancyName;
            entity.IsActive = updateInput.IsActive;
        }

        public async Task UpdateTenant(TenantDto updateInput)
        {
            var category = await _tenantCategoryManager.TenantCategories
                .Where(x => x.Title == updateInput.Category.Title)
                .Include(x => x.SubCategories)
                .OrderBy(x => x.Title).ToListAsync();
           
            //var tenant = await _tenantManager.FindByIdAsync(updateInput.Id);

            var tenant = await _tenantManager.Tenants.Where(x => x.Id == updateInput.Id).Include(x => x.SubCategories).Take(1).FirstOrDefaultAsync();
            tenant.Name = updateInput.Name;
            tenant.TenancyName = updateInput.TenancyName;
            tenant.IsActive = updateInput.IsActive;
            tenant.Category = category.FirstOrDefault();

            //tenant.ClearSubCategories();
            //SubCategories.Clear();


            tenant.ClearSubCategories();
            //tenant.SubCategories.Clear();
            //tenant.SubCategories = new List<TenantWithSubCategory>();
            //updateInput.SubCategories.Find(x => x.Title == )
            foreach (var subcategory in category.FirstOrDefault().SubCategories.ToList())
            {
                if (updateInput.SubCategories.Exists(x => x.Title == subcategory.Title))
                {
                    var twsc = new TenantWithSubCategory(tenant, subcategory);
                    await CurrentUnitOfWork.SaveChangesAsync();

                    tenant.AddSubCategories(twsc);

                }

            }
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        
        public override async Task Delete(EntityDto<int> input)
        {
            CheckDeletePermission();

            var tenant = await _tenantManager.GetByIdAsync(input.Id);
            await _tenantManager.DeleteAsync(tenant);
        }

        private void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        public async Task<ListResultDto<TenantDto>> GetTenants(GetTenantsInput input)
        {
            var tenants = await _tenantManager.Tenants
                .Where(x => (!input.Keyword.IsNullOrWhiteSpace()) ? x.TenancyName.Contains(input.Keyword) : true)
                .Where(x => (input.IsActive.HasValue) ? x.IsActive == input.IsActive : true)
                .Include(x => x.Category)
                .Include(x => x.SubCategories)
                //.Include(x => x.SubCategories)
                .OrderBy(x => x.TenancyName)
                .ToListAsync();

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
                        ProfilePictureId = x.ProfilePictureId,
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


            return new ListResultDto<TenantDto>(items);
        }

        public async Task<TenantDto> GetTenant(EntityDto<long> input)
        {
            var tenant = await _tenantManager.Tenants
                //.Where(x => (!input.Keyword.IsNullOrWhiteSpace()) ? x.TenancyName.Contains(input.Keyword) : true)
                .Where(x => x.Id == input.Id)
                .Include(x => x.Category)
                .Include(x => x.SubCategories)
                //.Include(x => x.SubCategories)
                .OrderBy(x => x.TenancyName)
                .ToListAsync();

            var items = tenant
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
                        ProfilePictureId = x.ProfilePictureId,
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


            return items.FirstOrDefault();
        }

    }


}

