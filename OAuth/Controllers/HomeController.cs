using System.Reflection;
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
            var assembly = Assembly.GetEntryAssembly();
            var attribute = assembly?.GetCustomAttribute<AssemblyFileVersionAttribute>();

            var name = assembly?.GetName().Name;
            var version = attribute?.Version;
            var message = $"{name} {version}";

            _logger.LogInformation("Index request. message: {0}", message);

            return Content(message);
        }
    }
}