﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Identity.Identities.Models;

namespace Ajupov.Identity.Identities.Services
{
    public interface IIdentityTokensService
    {
        Task<IdentityToken> GetAsync(Guid id, CancellationToken ct);

        Task<Guid> CreateAsync(IdentityToken token, CancellationToken ct);

        Task SetIsUsedAsync(Guid id, CancellationToken ct);
    }
}