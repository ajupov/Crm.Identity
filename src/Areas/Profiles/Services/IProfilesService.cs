using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Crm.Identity.Areas.Profiles.Models;
using Crm.Identity.Areas.Profiles.Requests;

namespace Crm.Identity.Areas.Profiles.Services
{
    public interface IProfilesService
    {
        Task<Profile> GetAsync(Guid id, CancellationToken ct);

        Task<List<Profile>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task<List<Profile>> GetPagedListAsync(ProfilesGetPagedListRequest request, CancellationToken ct);

        Task<Guid> CreateAsync(Profile profile, CancellationToken ct);

        Task UpdateAsync(Profile oldProfile, Profile profile, CancellationToken ct);

        Task LockAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task UnlockAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct);

        Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct);
    }
}