using Microsoft.AspNetCore.Antiforgery;
using VOU.Controllers;

namespace VOU.Web.Host.Controllers
{
    public class AntiForgeryController : VOUControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
