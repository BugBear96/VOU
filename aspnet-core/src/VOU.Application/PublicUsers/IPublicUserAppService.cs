
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;
using VOU.Dto;
using VOU.PublicUsers.Dto;

namespace VOU.PublicUsers
{
    public interface IPublicUserAppService : IApplicationService
    {
        Task<PagedResultDto<UserListDto>> GetPublicUsers(PagedResultInput input);
    }
}
