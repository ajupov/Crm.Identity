using Ajupov.Infrastructure.All.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Crm.Identity.OAuth.Controllers
{
    [Route("")]
    public class HomeController : DefaultMvcController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet("")]
        public ActionResult Index()
        {
            return new RedirectResult("~/swagger");
        }
    }
}