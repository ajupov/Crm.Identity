using Ajupov.Infrastructure.All.ApiDocumentation.Attributes;
using Ajupov.Infrastructure.All.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Identity.OAuth.Controllers
{
    [Route("")]
    public class HomeController : DefaultMvcController
    {
        [IgnoreApiDocumentation]
        [HttpGet("")]
        public ActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}
