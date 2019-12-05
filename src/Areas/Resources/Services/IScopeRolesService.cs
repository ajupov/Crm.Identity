using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crm.Identity.Areas.Resources.Services
{
    public interface IScopeRolesService
    {
        Task<Dictionary<string, string[]>> GetAsync();
    }
}