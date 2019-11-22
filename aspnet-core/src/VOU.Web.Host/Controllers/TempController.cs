using Abp.AspNetCore.Mvc.Authorization;
using Abp.Extensions;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace VOU.Web.Host.Controllers
{
    //[AbpMvcAuthorize]
    public class TempController : VOUControllerBase
    {

        private static readonly Regex UserProfileImageFnmRegex = new Regex(@"^userProfileImage_(?<userid>\d+).jpg$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex TenantProfilePictureFnmRegex = new Regex(@"^tenantProfilePicture_[A-Za-z0-9]{32}\.jpg$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex VoucherPlatformCoverPictureFnmRegex = new Regex(@"^voucherPlatformCoverPicture_[A-Za-z0-9]{32}\.jpg$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex BranchCoverPictureFnmRegex = new Regex(@"^branchCoverPicture_[A-Za-z0-9]{32}\.jpg$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        //[HttpGet]
        public ActionResult Downloads(string id)
        {
            if (id.IsNullOrEmpty())
                return NotFound();

            var m = UserProfileImageFnmRegex.Match(id);
            if (m.Success)
            {
                var userId = long.Parse(m.Groups["userid"].Value, CultureInfo.InvariantCulture);
                if (userId != AbpSession.GetUserId())
                    return NotFound();

                //return File(Path.Combine(Path.GetTempPath(), id), "image/jpeg");
                FileStream stream = new FileStream(Path.Combine(Path.GetTempPath(), id), FileMode.Open);
                FileStreamResult result = new FileStreamResult(stream, "image/jpeg");
                return result;
            }

            m = TenantProfilePictureFnmRegex.Match(id);
            if (m.Success)
            {
                //return File(Path.Combine(Path.GetTempPath(), id), "image/jpeg");
                FileStream stream = new FileStream(Path.Combine(Path.GetTempPath(), id), FileMode.Open);
                FileStreamResult result = new FileStreamResult(stream, "image/jpeg");
                return result;
            }

            m = VoucherPlatformCoverPictureFnmRegex.Match(id);
            if (m.Success)
            {
                //return File(Path.Combine(Path.GetTempPath(), id), "image/jpeg");
                FileStream stream = new FileStream(Path.Combine(Path.GetTempPath(), id), FileMode.Open);
                FileStreamResult result = new FileStreamResult(stream, "image/jpeg");
                return result;
            }

            m = BranchCoverPictureFnmRegex.Match(id);
            if (m.Success)
            {
                //return File(Path.Combine(Path.GetTempPath(), id), "image/jpeg");
                FileStream stream = new FileStream(Path.Combine(Path.GetTempPath(), id), FileMode.Open);
                FileStreamResult result = new FileStreamResult(stream, "image/jpeg");
                return result;
            }

            return NotFound();  
            
        }

    }
}
