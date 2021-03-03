using System.Threading.Tasks;
using Ajupov.Infrastructure.All.Configuration;
using Ajupov.Infrastructure.All.Hosting;
using Ajupov.Infrastructure.All.Logging;
using Microsoft.AspNetCore.Hosting;

namespace Crm.Identity
{
    public static class Program
    {
        public static Task Main()
        {
            var configuration = Configuration.GetConfiguration();

            return configuration
                .ConfigureHosting<Startup>()
                .ConfigureWebRoot()
                .ConfigureLogging(configuration)
                .Build()
                .RunAsync();
        }
    }
}
