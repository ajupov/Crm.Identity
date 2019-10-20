using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ajupov.Identity.src.Profiles.Helpers;
using Ajupov.Identity.src.Profiles.Models;
using Ajupov.Identity.src.Profiles.Requests;
using Ajupov.Identity.src.Profiles.Storages;
using Ajupov.Utils.All.String;
using Microsoft.EntityFrameworkCore;

namespace Ajupov.Identity.src.Profiles.Services
{
    public class ProfilesService : IProfilesService
    {
        private readonly ProfilesStorage _profilesStorage;

        public ProfilesService(ProfilesStorage profilesStorage)
        {
            _profilesStorage = profilesStorage;
        }

        public Task<Profile> GetAsync(Guid id, CancellationToken ct)
        {
            return _profilesStorage.Profiles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public Task<List<Profile>> GetListAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            return _profilesStorage.Profiles
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync(ct);
        }

        public Task<List<Profile>> GetPagedListAsync(ProfilesGetPagedListRequest request, CancellationToken ct)
        {
            return _profilesStorage.Profiles
                .AsNoTracking()
                .Where(x =>
                    (request.Surname.IsEmpty() || EF.Functions.Like(x.Surname, $"{request.Surname}%")) &&
                    (request.Name.IsEmpty() || EF.Functions.Like(x.Name, $"{request.Name}%")) &&
                    (!request.MinBirthDate.HasValue || x.BirthDate >= request.MinBirthDate) &&
                    (!request.MaxBirthDate.HasValue || x.BirthDate <= request.MaxBirthDate) &&
                    (!request.Gender.HasValue || x.Gender == request.Gender) &&
                    (!request.IsLocked.HasValue || x.IsLocked == request.IsLocked) &&
                    (!request.IsDeleted.HasValue || x.IsDeleted == request.IsDeleted) &&
                    (!request.MinCreateDate.HasValue || x.CreateDateTime >= request.MinCreateDate) &&
                    (!request.MaxCreateDate.HasValue || x.CreateDateTime <= request.MaxCreateDate) &&
                    (!request.MinModifyDate.HasValue || x.ModifyDateTime >= request.MinModifyDate) &&
                    (!request.MaxModifyDate.HasValue || x.ModifyDateTime <= request.MaxModifyDate))
                .Sort(request.SortBy, request.OrderBy)
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToListAsync(ct);
        }

        public async Task<Guid> CreateAsync(Profile profile, CancellationToken ct)
        {
            var newProfile = new Profile
            {
                Id = Guid.NewGuid(),
                Surname = profile.Surname,
                Name = profile.Name,
                BirthDate = profile.BirthDate,
                Gender = profile.Gender,
                IsLocked = false,
                IsDeleted = false,
                CreateDateTime = DateTime.UtcNow
            };

            var entry = await _profilesStorage.AddAsync(newProfile, ct);
            await _profilesStorage.SaveChangesAsync(ct);

            return entry.Entity.Id;
        }

        public Task UpdateAsync(Profile oldProfile, Profile profile, CancellationToken ct)
        {
            oldProfile.Surname = profile.Surname;
            oldProfile.Name = profile.Name;
            oldProfile.BirthDate = profile.BirthDate;
            oldProfile.Gender = profile.Gender;
            oldProfile.IsLocked = profile.IsLocked;
            oldProfile.IsDeleted = profile.IsDeleted;
            oldProfile.ModifyDateTime = DateTime.UtcNow;

            _profilesStorage.Update(oldProfile);

            return _profilesStorage.SaveChangesAsync(ct);
        }

        public async Task LockAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            await _profilesStorage.Profiles
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x =>
                {
                    x.IsLocked = true;
                    x.ModifyDateTime = DateTime.UtcNow;
                }, ct);

            await _profilesStorage.SaveChangesAsync(ct);
        }

        public async Task UnlockAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            await _profilesStorage.Profiles
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x =>
                {
                    x.IsLocked = false;
                    x.ModifyDateTime = DateTime.UtcNow;
                }, ct);

            await _profilesStorage.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            await _profilesStorage.Profiles
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x =>
                {
                    x.IsDeleted = true;
                    x.ModifyDateTime = DateTime.UtcNow;
                }, ct);

            await _profilesStorage.SaveChangesAsync(ct);
        }

        public async Task RestoreAsync(IEnumerable<Guid> ids, CancellationToken ct)
        {
            await _profilesStorage.Profiles
                .Where(x => ids.Contains(x.Id))
                .ForEachAsync(x =>
                {
                    x.IsDeleted = false;
                    x.ModifyDateTime = DateTime.UtcNow;
                }, ct);

            await _profilesStorage.SaveChangesAsync(ct);
        }
    }
}