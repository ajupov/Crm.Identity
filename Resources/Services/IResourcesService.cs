﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Crm.Identity.Resources.Services
{
    public interface IResourcesService
    {
        Task<List<string>> GetRolesByScopesAsync(IEnumerable<string> scopes, CancellationToken ct);
    }
}