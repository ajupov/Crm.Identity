using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Identity.Identities.Models;
using Ajupov.Identity.Identities.Requests;

namespace Ajupov.Identity.Identities.Services
{
    public interface IIdentitiesService
    {
        Task<Models.Identity> GetAsync(Guid id, CancellationToken ct);

        Task<Models.Identity[]> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<Models.Identity> GetVerifiedByKeyAndTypesAsync(
            string key,
            IEnumerable<IdentityType> types,
            CancellationToken ct);

        Task<bool> IsExistByKeyAndTypeAsync(string key, IdentityType type, CancellationToken ct);

        Task<Models.Identity[]> GetPagedListAsync(
            IdentitiesGetPagedListRequest request,
            CancellationToken ct);

        Task<Guid> CreateAsync(Models.Identity identity, CancellationToken ct);

        Task UpdateAsync(
            Models.Identity oldIdentity,
            Models.Identity identity,
            CancellationToken ct);

        Task SetPasswordAsync(Models.Identity identity, string password, CancellationToken ct);

        Task VerifyAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task UnverifyAsync(IEnumerable<Guid> ids, CancellationToken ct);

        bool IsPasswordCorrect(Models.Identity identity, string password);
    }
}