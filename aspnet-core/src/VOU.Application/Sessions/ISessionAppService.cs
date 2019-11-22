using System.Threading.Tasks;
using Abp.Application.Services;
using VOU.Sessions.Dto;

namespace VOU.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
