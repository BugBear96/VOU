using System.Threading.Tasks;
using Abp.Application.Services;
using VOU.Authorization.Accounts.Dto;

namespace VOU.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);

        Task RegisterUser(RegisterUserInput input);

        Task<string> RegisterPhone(RegisterUserInput input);

        Task<bool> VerifyPhone(RegisterUserInput input);
    }
}
