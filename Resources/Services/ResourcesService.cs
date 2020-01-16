using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Identity.Resources.Storages;
using Crm.Identity.Scopes;
using Microsoft.EntityFrameworkCore;

namespace Crm.Identity.Resources.Services
{
    public class ResourcesService : IResourcesService
    {
        private readonly ResourcesStorage _storage;

        public ResourcesService(ResourcesStorage storage)
        {
            _storage = storage;
        }

        public async Task<List<string>> GetRolesByScopesAsync(IEnumerable<string> scopes, CancellationToken ct)
        {
            var resources = await _storage.Resources.ToListAsync(ct);
            var resourceScopes = resources.Select(x => x.Scope);
            var resultScopes = resourceScopes.Intersect(scopes);

            return ScopesWithRoles.Value
                .Where(x => resultScopes.Contains(x.Key))
                .SelectMany(x => x.Value)
                .ToList();
        }
    }
}