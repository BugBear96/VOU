using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Uow;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VOU.BinaryObjects;
using VOU.Web.Host.Models;

namespace VOU.Web.Host.Controllers 
{
    [AbpMvcAuthorize]
    public class VoucherController : VOUControllerBase
    {
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private IHostingEnvironment _hostingEnvironment;

        public VoucherController(
            IBinaryObjectManager binaryObjectManager,
            IUnitOfWorkManager unitOfWorkManager,
            IHostingEnvironment environment)
        {
            _binaryObjectManager = binaryObjectManager;
            _unitOfWorkManager = unitOfWorkManager;
            _hostingEnvironment = environment;
        }

        [HttpPost]
        public async Task<IActionResult> UploadVoucherPlatformCoverPicture(IFormFile file)
        {

            var fnm = $"voucherPlatformCoverPicture_{Guid.NewGuid().ToString("n")}.jpg";
            var fullPath = Path.Combine(Path.GetTempPath(), fnm);


            using (var stream = System.IO.File.Create(fullPath))
            {

                await file.CopyToAsync(stream);

                var img = System.Drawing.Image.FromStream(stream);
                return Json(new UploadPictureViewModel
                {
                    Width = img.Width,
                    Height = img.Height,
                    FileName = fnm
                });


            }


        }

        [AllowAnonymous]
        public async Task<FileResult> GetVoucherPlatformCoverPictureById(long id)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var binaryObject = await _binaryObjectManager.GetAsync(id, BinaryObjectTypes.VoucherPlatformCoverPicture);

                if (binaryObject != null)
                    return File(binaryObject.Content, binaryObject.ContentType);

                return File(Path.Combine(_hostingEnvironment.WebRootPath, "/images/default-thumb.png"), "image/png");
            }
        }
    }
}
