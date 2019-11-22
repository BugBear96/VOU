

using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Uow;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using VOU.BinaryObjects;
using VOU.Web.Host.Models;
using static System.Net.Mime.MediaTypeNames;

namespace VOU.Web.Host.Controllers
{
    [AbpMvcAuthorize]
    public class TenantController : VOUControllerBase
    {

        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private IHostingEnvironment _hostingEnvironment;

        public TenantController(
            IBinaryObjectManager binaryObjectManager,
            IUnitOfWorkManager unitOfWorkManager,
            IHostingEnvironment environment)
        {
            _binaryObjectManager = binaryObjectManager;

            _unitOfWorkManager = unitOfWorkManager;
            _hostingEnvironment = environment;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                var fnm = $"tenantProfilePicture_{Guid.NewGuid().ToString("n")}.jpg";
                var fullPath = Path.Combine(Path.GetTempPath(), fnm);


                using (var stream = System.IO.File.Create(fullPath))
                {
                    

                    await file.CopyToAsync(stream);

                    var img = System.Drawing.Image.FromStream(stream);
                    //img.Save(fullPath, ImageFormat.Jpeg);
                    return Json(new UploadPictureViewModel
                    {
                        Width = img.Width,
                        Height = img.Height,
                        FileName = fnm
                    });


                }

            }
            catch(Exception e)
            {
                return Json(new UploadPictureViewModel
                {
                    
                });
            }
            
        }

        [AllowAnonymous]
        public async Task<FileResult> GetProfilePictureById(long id)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var binaryObject = await _binaryObjectManager.GetAsync(id, BinaryObjectTypes.TenantProfilePicture);

                if (binaryObject != null)
                    return File(binaryObject.Content, binaryObject.ContentType);
                
                return File(Path.Combine(_hostingEnvironment.WebRootPath, "/images/default-thumb.png"), "image/png");
            }
        }
    }
}
