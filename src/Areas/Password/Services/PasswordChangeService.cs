using System.Threading;
using System.Threading.Tasks;
using Crm.Identity.Areas.Identities.Extensions;
using Crm.Identity.Areas.Identities.Services;
using Crm.Identity.Areas.OAuth.Models.ChangePassword;
using Crm.Identity.Areas.Profiles.Services;

namespace Crm.Identity.Areas.Password.Services
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