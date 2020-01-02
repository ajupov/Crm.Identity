using System.Threading;
using System.Threading.Tasks;
using Ajupov.Utils.All.Phone;
using Crm.Identity.Identities.Extensions;
using Crm.Identity.Identities.Models;
using Crm.Identity.Identities.Services;
using Crm.Identity.OAuth.Models.ChangePassword;
using Crm.Identity.Profiles.Services;

namespace Crm.Identity.Password.Services
{
    public class PasswordChangeService : IPasswordChangeService
    {
        private readonly IProfilesService _profilesService;
        private readonly IIdentitiesService _identitiesService;

        public PasswordChangeService(IProfilesService profilesService, IIdentitiesService identitiesService)
        {
            _profilesService = profilesService;
            _identitiesService = identitiesService;
        }

        public async Task<PostChangePasswordResponse> ChangeAsync(
            string country,
            string key,
            string oldPassword,
            string newPassword,
            CancellationToken ct)
        {
            var identityTypes = IdentityTypeExtensions.TypesWithPassword;
            var phoneIdentityType = new[] {IdentityType.PhoneAndPassword};

            var identity = await _identitiesService.GetVerifiedByKeyAndTypesAsync(key, identityTypes, ct) ??
                           await _identitiesService.GetVerifiedByKeyAndTypesAsync(key.GetPhoneWithoutPrefixes(country),
                               phoneIdentityType, ct);
            
            if (identity == null)
            {
                return new PostChangePasswordResponse(true);
            }

            var profile = await _profilesService.GetAsync(identity.ProfileId, ct);
            if (profile == null)
            {
                return new PostChangePasswordResponse(true);
            }

            var isPasswordCorrect = _identitiesService.IsPasswordCorrect(identity, oldPassword);
            if (!isPasswordCorrect)
            {
                return new PostChangePasswordResponse(true);
            }

            await _identitiesService.ChangePasswordByProfileIdAsync(profile.Id, newPassword, ct);

            return new PostChangePasswordResponse(false);
        }
    }
}