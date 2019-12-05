using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Identity.Areas.Identities.Models;
using Crm.Identity.Areas.Identities.Requests;

namespace Crm.Identity.Areas.Identities.Services
{
    public interface IIdentitiesService
    {
        Task<Models.Identity> GetAsync(Guid id, CancellationToken ct);

        Task<Models.Identity[]> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<Models.Identity> GetByKeyAndTypesAsync(
            string key,
            IEnumerable<IdentityType> types,
            CancellationToken ct);

        Task<Models.Identity> GetVerifiedByKeyAndTypesAsync(
            string key,
            IEnumerable<IdentityType> types,
            CancellationToken ct);

        Task<Models.Identity> GetByProfileIdAndTypeAsync(
            Guid profileId,
            IdentityType type,
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

        Task ChangePasswordByProfileIdAsync(Guid profileId, string newPassword, CancellationToken ct);
    }
}