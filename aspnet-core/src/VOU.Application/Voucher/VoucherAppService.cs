using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOU.Voucher.Dto;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using VOU.BinaryObjects;
using VOU.Dto;

namespace VOU.Voucher
{
    public class VoucherAppService : VOUAppServiceBase, IVoucherAppService
    {
        private readonly IVoucherPlatformManager _voucherPlatformManager;
        private readonly IBinaryObjectManager _binaryObjectManager;

        public VoucherAppService(
            IVoucherPlatformManager voucherPlatformManager,
            IBinaryObjectManager binaryObjectManager)
        {
            _voucherPlatformManager = voucherPlatformManager;
            _binaryObjectManager = binaryObjectManager;
        }

        public async Task ArchiveVoucherPlatform(EntityDto input)
        {
            var platform = await _voucherPlatformManager.FindAsync(input.Id);
            if (platform == null)
                throw new UserFriendlyException(L("InvalidVoucherPlatform"));

            platform.Archive(AbpSession.UserId);
        }

        public async Task ActivateVoucherPlatform(EntityDto input)
        {
            var platform = await _voucherPlatformManager.FindAsync(input.Id, considerArchived: true);
            if (platform == null)
                throw new UserFriendlyException(L("InvalidVoucherPlatform"));

            var existingPlatform = await _voucherPlatformManager.FindByNameAsync(platform.Name);
            if (existingPlatform != null)
                throw new UserFriendlyException(L("AnotherVoucherPlatformWithSameNameExists"));

            platform.Activate();
        }

        public async Task<ListResultDto<VoucherPlatformListDto>> GetVoucherPlatforms(GetVoucherPlatformInput input)
        {
            try
            {
                var items = await _voucherPlatformManager.VoucherPlatforms
                .Where(x => (!input.Keyword.IsNullOrWhiteSpace()) ? x.Name.Contains(input.Keyword) : true)
                .WhereIf(input.ShowArchived, x => x.ArchivedTime != null)
                .WhereIf(!input.ShowArchived, x => x.ArchivedTime == null)
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
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }
            return new ListResultDto<VoucherPlatformListDto>();

        }

        public async Task<VoucherPlatformEditDto> GetVoucherPlatformForEdit(EntityDto input)
        {

            VoucherPlatform platform;

            if (input.Id > 0)
            {
                platform = await _voucherPlatformManager.FindAsync(input.Id, considerArchived: true);
                if (platform == null)
                    throw new UserFriendlyException(L("InvalidVoucherPlatform"));
            }
            else platform = new VoucherPlatform("New Voucher Platform");

            var dto = ObjectMapper.Map<VoucherPlatformEditDto>(platform);
            dto.TermConditionJson = platform.GetSettings();

            return dto;

        }

        public async Task<EntityDto> CreateOrUpdateVoucherPlatform(VoucherPlatformEditDto input)
        {
            try
            {
                var isEdit = input.Id > 0;

                VoucherPlatform platform;
                if (isEdit)
                {
                    platform = await _voucherPlatformManager.FindAsync(input.Id);
                    if (platform == null)
                        throw new UserFriendlyException(L("InvalidVoucherPlatform"));
                }
                else platform = new VoucherPlatform(input.Name);

                platform.UpdateName(input.Name);
                platform.UpdateSettings(input.TermConditionJson);

                if (!isEdit)
                    await _voucherPlatformManager.CreateAsync(platform);
                else
                    await _voucherPlatformManager.UpdateVoucherPlatformAsync(platform);

                await CurrentUnitOfWork.SaveChangesAsync();

                return new EntityDto(platform.Id);
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }

            return new EntityDto();
        }

        //[AbpAuthorize(PermissionNames.Administration_Facilities_Manage)]
        public async Task<UpdateCoverPictureOutput> UpdateVoucherPlatformCoverPicture(UpdateCoverPictureInput input)
        {
            var platform = await _voucherPlatformManager.FindAsync(input.TargetId);
            if (platform == null)
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

            var binaryObjectId = await _binaryObjectManager.SaveAsync(BinaryObjectTypes.VoucherPlatformCoverPicture, data, "image/jpeg");

            if (platform.CoverPictureId != null)
                await _binaryObjectManager.DeleteAsync(platform.CoverPictureId.Value, BinaryObjectTypes.TenantProfilePicture);

            platform.UpdateCoverPicture(binaryObjectId);
            await _voucherPlatformManager.UpdateVoucherPlatformAsync(platform);

            return new UpdateCoverPictureOutput
            {
                TargetId = platform.Id,
                CoverPictureId = platform.CoverPictureId
            };
        }
    }
}
