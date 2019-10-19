using System.Threading;
using System.Threading.Tasks;
using Ajupov.Identity.Identities.Models;
using Ajupov.Identity.Identities.Services;
using Ajupov.Identity.OAuth.Models.ChangeEmail;
using Ajupov.Identity.Profiles.Services;

namespace Ajupov.Identity.Email.Services
{
    public class EmailChangeService : IEmailChangeService
    {
        private readonly IProfilesService _profilesService;
        private readonly IIdentitiesService _identitiesService;
        private readonly IEmailConfirmationService _emailConfirmationService;

        public EmailChangeService(
            IProfilesService profilesService,
            IIdentitiesService identitiesService,
            IEmailConfirmationService emailConfirmationService)
        {
            _profilesService = profilesService;
            _identitiesService = identitiesService;
            _emailConfirmationService = emailConfirmationService;
        }

        public async Task<PostChangeEmailResponse> ChangeAsync(
            string oldEmail,
            string newEmail,
            string password,
            string userAgent,
            string ipAddress,
            CancellationToken ct)
        {
            var identityTypes = new[] {IdentityType.EmailAndPassword};
            var identity = await _identitiesService.GetVerifiedByKeyAndTypesAsync(oldEmail, identityTypes, ct);
            if (identity == null)
            {
                return new PostChangeEmailResponse(true);
            }

            var profile = await _profilesService.GetAsync(identity.ProfileId, ct);
            if (profile == null)
            {
                return new PostChangeEmailResponse(true);
            }

            var isPasswordCorrect = _identitiesService.IsPasswordCorrect(identity, password);
            if (!isPasswordCorrect)
            {
                return new PostChangeEmailResponse(true);
            }

            var newIdentity = new Identities.Models.Identity
            {
                Key = newEmail
            };

            await _identitiesService.UpdateAsync(identity, newIdentity, ct);
            await _emailConfirmationService.SendMessageAsync(newEmail, ipAddress, userAgent, ct);

            return new PostChangeEmailResponse(false);
        }
    }
}