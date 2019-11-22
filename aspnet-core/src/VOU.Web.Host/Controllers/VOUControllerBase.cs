
using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Abp.UI;
using Microsoft.AspNetCore.Identity;

namespace VOU.Web.Host.Controllers
{
    public class VOUControllerBase : AbpController
    {
        protected VOUControllerBase()
        {
            LocalizationSourceName = VOUConsts.LocalizationSourceName;
        }

        protected virtual void CheckModelState()
        {
            if (!ModelState.IsValid)
            {
                throw new UserFriendlyException(L("FormIsNotValidMessage"));
            }
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

    }
}
