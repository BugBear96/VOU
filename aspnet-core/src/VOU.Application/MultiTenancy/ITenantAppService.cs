using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;
using VOU.MultiTenancy.Dto;

namespace VOU.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
        Task UpdateTenant(TenantDto updateInput);
        Task<ListResultDto<TenantDto>> GetTenants(GetTenantsInput input);
        Task<TenantDto> GetTenant(EntityDto<long> input);
    }
}

