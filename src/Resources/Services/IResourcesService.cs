using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ajupov.Identity.src.Resources.Services
{
    public interface IResourcesService
    {
        Task<List<string>> GetRolesByScopesAsync(IEnumerable<string> scopes, CancellationToken ct);
    }
}