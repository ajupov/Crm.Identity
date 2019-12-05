using System.Reflection;
using Ajupov.Infrastructure.All.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Identity.Areas.OAuth.Controllers
{
    [Route("")]
    public class HomeController : DefaultMvcController
    {
        [HttpGet("")]
        public ActionResult Index()
        {
            var assembly = Assembly.GetEntryAssembly();
            var attribute = assembly?.GetCustomAttribute<AssemblyFileVersionAttribute>();

            var name = assembly?.GetName()?.Name;
            var version = attribute?.Version;

            return Content($"{name} {version}");
        }
    }
}