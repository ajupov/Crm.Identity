using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crm.Identity.Resources.Services
{
    public interface IScopeRolesService
    {
        Task<Dictionary<string, string[]>> GetAsync();
    }
}