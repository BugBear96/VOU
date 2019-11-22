using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;
using VOU.MultiTenancy.Dto;
using VOU.Partners.Dto;

namespace VOU.Partners
{
    public interface IPartnerAppService : IApplicationService
    {
        Task<UpdateProfilePictureOutput> UpdateIconPicture(UpdateProfilePictureInput input);

        Task<TenantDto> GetTenant(EntityDto<long> input);
    }
}
