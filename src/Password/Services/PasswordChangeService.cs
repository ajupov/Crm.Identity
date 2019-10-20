using System.Threading;
using System.Threading.Tasks;
using Ajupov.Identity.src.Identities.Extensions;
using Ajupov.Identity.src.Identities.Services;
using Ajupov.Identity.src.OAuth.Models.ChangePassword;
using Ajupov.Identity.src.Profiles.Services;

namespace Ajupov.Identity.src.Password.Services
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
            string login,
            string oldPassword,
            string newPassword,
            CancellationToken ct)
        {
            var identityTypes = IdentityTypeExtensions.TypesWithPassword;
            var identity = await _identitiesService.GetVerifiedByKeyAndTypesAsync(login, identityTypes, ct);
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