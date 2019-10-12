using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Identity.Resources.Storages;

namespace Ajupov.Identity.Resources.Services
{
    public class ResourcesService : IResourcesService
    {
        private readonly ResourcesStorage _storage;
        private readonly IScopePermissionsService _scopePermissionsService;

        public ResourcesService(ResourcesStorage storage, IScopePermissionsService scopePermissionsService)
        {
            _storage = storage;
            _scopePermissionsService = scopePermissionsService;
        }

        public async Task<List<string>> GetRolesByScopesAsync(IEnumerable<string> scopes, CancellationToken ct)
        {
            var scopePermissions = await _scopePermissionsService.GetAsync();
            var intersectedScopes = _storage.Resources
                .Select(x => x.Scope)
                .Intersect(scopes);

            return scopePermissions
                .Where(x => intersectedScopes.Contains(x.Key))
                .SelectMany(x => x.Value)
                .ToList();
        }
    }
}