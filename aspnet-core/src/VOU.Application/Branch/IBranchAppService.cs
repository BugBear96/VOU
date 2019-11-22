using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VOU.Branch.Dto;
using VOU.Dto;

namespace VOU.Branch
{
    public interface IBranchAppService : IApplicationService
    {
        Task<ListResultDto<StateListDto>> GetStates(GetStateInput input);

        Task<StateEditDto> GetStateForEdit(EntityDto input);

        Task<EntityDto> CreateOrUpdateState(StateEditDto input);

        Task<ListResultDto<LocationListDto>> GetLocations(GetLocationInput input);

        Task<LocationEditDto> GetLocationForEdit(EntityDto input);

        Task<EntityDto> CreateOrUpdateLocation(LocationEditDto input);

        Task<ListResultDto<BranchWithVoucherPlatformListDto>> GetVoucherPlatformByBranch(EntityDto input);

        Task<BranchWithVoucherPlatformEditDto> GetVoucherPlatformForEdit(EntityDto input);

        Task<EntityDto> CreateOrUpdateVoucherPlatform(BranchWithVoucherPlatformEditDto input);

        Task<UpdateCoverPictureOutput> UpdateCoverPicture(UpdateCoverPictureInput input);
    }
}
