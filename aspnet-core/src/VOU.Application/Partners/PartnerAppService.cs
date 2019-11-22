using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOU.BinaryObjects;
using VOU.MultiTenancy;
using VOU.MultiTenancy.Dto;
using VOU.Partners.Dto;
using VOU.TenantCategories;
using VOU.TenantCategories.Dto;

namespace VOU.Partners
{
    public class PartnerAppService : VOUAppServiceBase, IPartnerAppService
    {
        private readonly TenantManager _tenantManager;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ITenantCategoryManager _tenantCategoryManager;

        public PartnerAppService(
            TenantManager tenantManager,
            IBinaryObjectManager binaryObjectManager,
            ITenantCategoryManager tenantCategoryManager)
        {
            _tenantManager = tenantManager;
            _binaryObjectManager = binaryObjectManager;
            _tenantCategoryManager = tenantCategoryManager;
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


        //[AbpAuthorize(PermissionNames.Administration_Facilities_Manage)]
        public async Task<UpdateProfilePictureOutput> UpdateIconPicture(UpdateProfilePictureInput input)
        {
            var tenant = await _tenantManager.FindByIdAsync(input.TenantId);
            if (tenant == null)
                throw new UserFriendlyException(L("InvalidAction"));

            byte[] data;
            using (var ms = new MemoryStream())
            {
                var newWidth = input.Width;
                var newHeight = input.Height;

                using (var newImg = new Bitmap(newWidth, newHeight))
                {
                    var fullPath = Path.Combine(Path.GetTempPath(), input.FileName);
                    using (var img = Image.FromFile(fullPath))
                    using (var g = Graphics.FromImage(newImg))
                    {
                        g.DrawImage(img,
                            new Rectangle(0, 0, newWidth, newHeight),
                            new Rectangle(input.X, input.Y, input.Width, input.Height),
                            GraphicsUnit.Pixel);
                    }
                    newImg.Save(ms, ImageFormat.Jpeg);
                }

                data = ms.ToArray();
            }

            var binaryObjectId = await _binaryObjectManager.SaveAsync(BinaryObjectTypes.TenantProfilePicture, data, "image/jpeg");

            if (tenant.ProfilePictureId != null)
                await _binaryObjectManager.DeleteAsync(tenant.ProfilePictureId.Value, BinaryObjectTypes.TenantProfilePicture);

            tenant.UpdateProfilePicture(binaryObjectId);
            await _tenantManager.UpdateAsync(tenant);

            return new UpdateProfilePictureOutput
            {
                TenantId = tenant.Id,
                ProfilePictureId = tenant.ProfilePictureId
            };
        }
    }
}
