using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ajupov.Identity.Resources.Services
{
    public interface IScopePermissionsService
    {
        Task<Dictionary<string, string[]>> GetAsync();
    }
}