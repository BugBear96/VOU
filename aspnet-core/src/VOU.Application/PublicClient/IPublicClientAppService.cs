using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VOU.Branch.Dto;
using VOU.PublicClient.Dto;
using VOU.Voucher.Dto;

namespace VOU.PublicClient
{
    public interface IPublicClientAppService : IApplicationService
    {
        Task<ListResultDto<TenantWithCategoryListDto>> GetTenants();

        Task<TenantBranchesListDto> GetLocations(EntityDto input);

        Task<ListResultDto<VoucherPlatformListDto>> GetVoucherPlatforms(EntityDto input);
    }
}
