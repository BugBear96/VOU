using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VOU.Dto;
using VOU.Voucher.Dto;

namespace VOU.Voucher
{
    public interface IVoucherAppService : IApplicationService
    {
        Task ArchiveVoucherPlatform(EntityDto input);

        Task ActivateVoucherPlatform(EntityDto input);

        Task<ListResultDto<VoucherPlatformListDto>> GetVoucherPlatforms(GetVoucherPlatformInput input);

        Task<VoucherPlatformEditDto> GetVoucherPlatformForEdit(EntityDto input);

        Task<EntityDto> CreateOrUpdateVoucherPlatform(VoucherPlatformEditDto input);

        Task<UpdateCoverPictureOutput> UpdateVoucherPlatformCoverPicture(UpdateCoverPictureInput input);

    }
}
