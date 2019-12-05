using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Crm.Identity.Areas.Resources.Storages;
using Microsoft.EntityFrameworkCore;

namespace Crm.Identity.Areas.Resources.Services
{
    public class ResourcesService : IResourcesService
    {
        private readonly ResourcesStorage _storage;
        private readonly IScopeRolesService _scopeRolesService;

        public ResourcesService(ResourcesStorage storage, IScopeRolesService scopeRolesService)
        {
            _storage = storage;
            _scopeRolesService = scopeRolesService;
        }

        public async Task<List<string>> GetRolesByScopesAsync(IEnumerable<string> scopes, CancellationToken ct)
        {
            var scopeRoles = await _scopeRolesService.GetAsync();
            var resourceScopes = await _storage.Resources.ToListAsync(ct);

            var intersectedScopes = resourceScopes
                .Select(x => x.Scope)
                .Intersect(scopes);

            return scopeRoles
                .Where(x => intersectedScopes.Contains(x.Key))
                .SelectMany(x => x.Value)
                .ToList();
        }
    }
}