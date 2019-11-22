using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace VOU.Controllers
{
    public abstract class VOUControllerBase: AbpController
    {
        protected VOUControllerBase()
        {
            LocalizationSourceName = VOUConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
